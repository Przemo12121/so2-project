namespace ThreadingLogic.Map;

public class GoCart : IRouteAccessor
{
    private readonly Route _route;
    public string Marker { get; set; } = String.Empty;
    private Section? Position { get; set; } = default;
    
    public int Delay { get; set; }
    
    public GoCart(Route route)
    {
        _route = route;
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
                    Position.Occupant = Marker;
                    Thread.Sleep(Delay);
                    nextPosition = _route.Next(Position);

                    lock (nextPosition)
                    {
                        nextPosition.Occupant = Marker;
                        Position!.Occupant = String.Empty;
                        Position = nextPosition;
                    }
                }
            }

            lock (Position)
            {
                Position.Occupant = Marker;
                Thread.Sleep(Delay);
                Position.Occupant = String.Empty;
            }
        }
        
    }
}
