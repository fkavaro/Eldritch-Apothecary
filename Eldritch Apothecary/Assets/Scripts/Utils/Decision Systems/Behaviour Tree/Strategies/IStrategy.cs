
/// <summary>
/// IStrategy interface defines the contract for all strategies used in the behaviour tree nodes.
/// </summary>
public interface IStrategy<TController>
where TController : ABehaviourController<TController>
{
    Node<TController>.Status Update();
    virtual void Reset() { }
}
