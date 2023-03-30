namespace ThreadingLogic.Map;

public class GoCart : IGoCart
{
    private readonly Route _route;
    public string Marker { get; set; } = String.Empty;
    public string HexColor { get; init; }
    private Section? Position { get; set; } = default;
    
    public int Delay { get; set; }
    
    public GoCart(Route route, string hexColor)
    {
        _route = route;
        HexColor = hexColor;
    }
    
    public void DoRounds(int rounds, CancellationToken cancellationToken)
    {
        if (rounds < 1)
        {
            return;
        }
        Section nextPosition;

        for (int lap = 0; lap < rounds && !cancellationToken.IsCancellationRequested; lap++)
        {
            Position = _route.Enter();
            
            while (_route.IsFinished(Position) is false)
            {
                lock (Position)
                {
                    Position.Occupant = this;
                    Thread.Sleep(Delay);
                    nextPosition = _route.Next(Position);

                    lock (nextPosition)
                    {
                        nextPosition.Occupant = this;
                        Position!.Occupant = default;
                        Position = nextPosition;
                    }
                }
            }

            lock (Position)
            {
                Position.Occupant = this;
                Thread.Sleep(Delay);
                Position.Occupant = default;
            }
        }
        
    }
}
