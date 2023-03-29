namespace ThreadingLogic;

public class Counter
{
    public int Value { get; private set; }
    
    public Counter(int initialValue)
    {
        Value = initialValue;
    }

    public void Decrease() => Value--;
    public void Increase() => Value++;
}