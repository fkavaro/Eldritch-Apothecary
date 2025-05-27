using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    Shelf _shopShelf;
    int _amountNeeded;
    bool _amountTaken = false;

    public Shopping_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Shopping", sfsm) { }

    public override void StartState()
    {
        _amountTaken = false;
        _controller.secondsWaiting = 0f;
        _controller.normalisedWaitingTime = 0f;

        _amountNeeded = Random.Range(10, 21); // Random amount needed
        _shopShelf = ApothecaryManager.Instance.RandomShopShelf();
        _controller.SetDestinationSpot(_shopShelf);
    }

    public override void UpdateState()
    {
        // Has reached exact position
        if (_controller.HasArrivedAtDestination())
        {
            if (!_amountTaken)
            {
                // Take needed amount from shelf
                if (_shopShelf.Take(_amountNeeded))
                {
                    _amountTaken = true;
                    // Go to waiting queue after animation
                    SwitchStateAfterRandomTime(_controller.waitForReceptionistState, _controller.pickUpAnim, "Picking up objects");
                }
                else
                {
                    // Wait
                    _controller.ChangeAnimationTo(_controller.waitAnim);
                    _controller.secondsWaiting += Time.deltaTime;
                    _controller.animationText.text = "Waiting for replenishment";
                }
            }
        }
        // Is close to the shelves spot
        else if (_controller.IsCloseToDestination())
        {
            // Shelves spot is occupied or not enough supply for needed amount
            if (_controller.DestinationSpotIsOccupied() || !_shopShelf.CanTake(_amountNeeded))
            {
                // Stop and wait
                _controller.SetIfStopped(true);
                _controller.ChangeAnimationTo(_controller.waitAnim);
                _controller.secondsWaiting += Time.deltaTime;

                // Not enough supply
                if (!_shopShelf.CanTake(_amountNeeded))
                    _controller.animationText.text = "Waiting for replenishment";
            }
            // Spot is free and has enough supplies
            else if (!_controller.DestinationSpotIsOccupied() && _shopShelf.CanTake(_amountNeeded))
            {
                _controller.SetIfStopped(false);
                _controller.ChangeAnimationTo(_controller.walkAnim);
            }
        }
    }

    public override void ExitState()
    {
        _amountTaken = false;
        _controller.secondsWaiting = 0f;
        _controller.normalisedWaitingTime = 0f;
        _controller.animationText.text = "";
    }
}
