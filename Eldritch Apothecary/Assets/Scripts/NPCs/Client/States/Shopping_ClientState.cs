using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : ANPCState<Client, FiniteStateMachine<Client>>
{
    int _amountNeeded, _shopAgainProbability;
    Shelf _shopShelf;
    List<Shelf> _visitedShelves = new();


    public Shopping_ClientState(FiniteStateMachine<Client> fsm)
    : base("Shopping", fsm) { }

    public override void StartState()
    {
        _controller.ResetWaitingTime();

        _amountNeeded = Random.Range(5, 16); // Random amount needed
        _shopShelf = ApothecaryManager.Instance.RandomShopShelf();
        _visitedShelves.Add(_shopShelf);
        _controller.SetDestinationSpot(_shopShelf);
        _controller.ChangeAnimationTo(_controller.walkAnim);

        // Probability of going to other shelf after picking up according to wanted service
        if (_controller.wantedService == Client.WantedService.SHOPPING)
            _shopAgainProbability = 5; // Clients that only want to shop will shop again  more probably
        else
            _shopAgainProbability = 1; // Less likely to shop again after picking up
    }

    public override void UpdateState()
    {
        // Has reached exact position
        if (_controller.HasArrivedAtDestination())
        {
            // Take needed amount from shelf
            if (_shopShelf.Take(_amountNeeded))
            {
                if (Random.Range(0, 11) <= _shopAgainProbability)
                    _controller.PlayAnimationRandomTime(_controller.pickUpAnim, "Picking up goods", GoToOtherShelf);
                else
                {
                    // Client is a SHOPLIFTER
                    if (_controller.personality == Client.Personality.SHOPLIFTER)
                        SwitchStateAfterRandomTime(_controller.fsmAction.robbingState, _controller.pickUpAnim, "Picking up goods");
                    // Is legal
                    else
                        // Go to waiting queue after animation
                        SwitchStateAfterRandomTime(_controller.fsmAction.waitForReceptionistState, _controller.pickUpAnim, "Picking up goods");
                }
            }
            else
            {
                // Wait
                _controller.ChangeAnimationTo(_controller.waitAnim);
                _controller.secondsWaiting += Time.deltaTime;
                _controller.animationText.text = "Waiting for replenishment";
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
            // Shelf is free and has enough supplies
            else if (!_controller.DestinationSpotIsOccupied() && _shopShelf.CanTake(_amountNeeded))
            {
                _controller.SetIfStopped(false);
                _controller.ChangeAnimationTo(_controller.walkAnim);
            }
        }
    }

    public override void ExitState()
    {
        _controller.ResetWaitingTime();
        _controller.animationText.text = "";
    }

    void GoToOtherShelf()
    {
        _controller.ResetWaitingTime();

        _amountNeeded = Random.Range(5, 16); // Random amount needed
        _shopShelf = ApothecaryManager.Instance.RandomShopShelf();
        while (_visitedShelves.Contains(_shopShelf)) // Check that it's new
            _shopShelf = ApothecaryManager.Instance.RandomShopShelf();
        _visitedShelves.Add(_shopShelf);
        _controller.SetDestinationSpot(_shopShelf);
        _controller.ChangeAnimationTo(_controller.walkAnim);
    }
}
