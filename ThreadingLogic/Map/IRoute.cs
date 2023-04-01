namespace ThreadingLogic.Map;

public interface IRoute<T>
{
    IReadOnlyList<T> Map { get; }
    T Next(T current);
    bool IsFinished(T current);
    T Enter();
}