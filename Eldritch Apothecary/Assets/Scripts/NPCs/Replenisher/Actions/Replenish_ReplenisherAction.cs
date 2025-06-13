using System;
using System.Collections.Generic;
using UnityEngine;

public class Replenish_ReplenisherAction : ALinearAction<Replenisher>
{
    #region PROPERTIES
    List<Shelf> _supplyShelves,
        _shelvesToResupply;
    bool _goAfterRobber = false;
    #endregion

    #region NODES
    BehaviourTree<Replenisher> _replenishBT;
    SequenceNode<Replenisher> _behaviourSequence;
    LeafNode<Replenisher> _takeSuppliesLeaf, _resupplyLeaf;
    #endregion

    #region STRATEGIES
    TakeSuppliesStrategy _takeSuppliesStrategy;
    ResupplyStrategy _resupplyStrategy;
    #endregion

    public Replenish_ReplenisherAction(string name, UtilitySystem<Replenisher> utilitySystem, List<Shelf> shelvesToResupply, List<Shelf> supplyShelves, bool goAfterRobber = false)
    : base(name, utilitySystem)
    {
        _shelvesToResupply = shelvesToResupply;
        _supplyShelves = supplyShelves;
        _goAfterRobber = goAfterRobber;
    }

    protected override float SetDecisionFactor()
    {
        // Normalised value of lacking supplies respect to the total amount that can be stored
        return ApothecaryManager.Instance.GetNormalisedLack(_shelvesToResupply);
    }

    public override void StartAction()
    {
        int lackingAmount = ApothecaryManager.Instance.GetTotalLack(_shelvesToResupply);

        // Strategies
        _takeSuppliesStrategy = new(_controller, _supplyShelves, lackingAmount);
        _resupplyStrategy = new(_controller, _shelvesToResupply);

        // Build BT
        _takeSuppliesLeaf = new(_controller, "Taking supplies", _takeSuppliesStrategy);
        _resupplyLeaf = new(_controller, "Resupplying shelves", _resupplyStrategy);
        _behaviourSequence = new(_controller);
        _behaviourSequence.AddChild(_takeSuppliesLeaf);
        _behaviourSequence.AddChild(_resupplyLeaf);
        _replenishBT = new(_controller, _behaviourSequence);
    }

    public override void UpdateAction()
    {
        // Update BT
        _replenishBT.Update();

        if (_replenishBT.status == Node<Replenisher>.Status.Success && _controller.debugMode)
            Debug.Log("Replenisher action completed successfully");
    }

    public override bool IsFinished()
    {
        return _replenishBT.status != Node<Replenisher>.Status.Running || // Behaviour tree has finished its sequence or failed
            (_goAfterRobber && ApothecaryManager.Instance.isSomeoneRobbing); // Will go after robber an someone is robbing
    }
}