using UnityEngine;

/// <summary>
/// Casts a spell for the client
/// </summary>
public class AttendingClients_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    int spellCastingTime;
    int failedSpell;
    int failedSpellConsequence;

    private float waitTimer = 0f;
    private bool isWaiting = false;

    public AttendingClients_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
    : base("Attending clients", sfsm) { }

    public override void StartState()
    {
        _controller.SetDestinationSpot(ApothecaryManager.Instance.sorcererSeat);

        // Takes a certain amount of time to cast a spell depending on personality
        switch (_controller.personality)
        {
            case Sorcerer.Personality.NORMAL:
                spellCastingTime = Random.Range(5, 10);
                break;

            case Sorcerer.Personality.ENERGISED:
                spellCastingTime = Random.Range(3, 5);
                break;

            case Sorcerer.Personality.LAZY:
                spellCastingTime = Random.Range(10, 15);
                break;
        }

        // Changes the chance to fail a spell according to skill
        switch (_controller.skill)
        {
            case Sorcerer.Skill.NOVICE:
                failedSpell = Random.Range(1, 4);
                break;

            case Sorcerer.Skill.ADEPT:
                failedSpell = Random.Range(1, 6);
                break;

            case Sorcerer.Skill.MASTER:
                failedSpell = Random.Range(1, 8);
                break;
        }
    }

    public override void UpdateState()
    {
        if (_controller.HasArrivedAtDestination())
        {
            if (!isWaiting)
            {
                _controller.ChangeAnimationTo(_controller.sitDownAnim);
                _controller.ChangeAnimationTo(_controller.castSpellAnim);

                // Casts spell for a certain amount of time
                _controller.CastSpellEffect(spellCastingTime);
                isWaiting = true;
                waitTimer = 0f;
            }

            // Timer to track the spell duration
            waitTimer += Time.deltaTime;

            // When sorcerer stops casting spell
            if (waitTimer >= spellCastingTime)
            {
                // If spell is failed
                if (failedSpell == 1)
                {
                    if (_controller.debugMode) Debug.Log("Failed Spell");

                    failedSpellConsequence = UnityEngine.Random.Range(0, 3);

                    // Makes client either bigger or smaller or changes skin color
                    switch (failedSpellConsequence)
                    {
                        case 0:
                            ApothecaryManager.Instance.sorcererClientsQueue[0].Shrink();
                            break;
                        case 1:
                            ApothecaryManager.Instance.sorcererClientsQueue[0].Enlarge();
                            break;
                        case 2:
                            ApothecaryManager.Instance.sorcererClientsQueue[0].ChangeColor();
                            break;
                    }

                    // Resets timer
                    waitTimer = 0f;
                    isWaiting = false;

                    // Starts from the beginning
                    SwitchState(_controller.pickUpIngredientsState);
                    return;
                }

                // If spell is successful, resets
                failedSpell = 0;
                // Adds one to the sorcerer turn
                ApothecaryManager.Instance.NextSorcererTurn();
                // Sorcerer waits for the next client
                SwitchState(_controller.waitForClientState);

                // Restarts timer
                waitTimer = 0f;
                isWaiting = false;
            }
        }
    }
}
