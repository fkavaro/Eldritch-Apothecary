
public interface IAction
{
    public string Name { get; }
    public float Utility { get; }
    public abstract void StartAction();
    public abstract void UpdateAction();
    public abstract bool IsFinished();
}
