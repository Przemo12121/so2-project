namespace ThreadingLogic.Utils;

public interface ICounter
{
    int Value { get; }
    void Increase();
    void Decrease();
}