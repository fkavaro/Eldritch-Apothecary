
/// <summary>
/// IStrategy interface defines the contract for all strategies used in the behaviour tree nodes.
/// </summary>
public interface IStrategy
{
    Node.Status Update();
    virtual void Reset() { }
}
