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
        
        int lap = 0;
        Section nextPosition = _route.Enter();
        Position = _route.Enter();
        
        do
        {
            lock (Position)
            {
                Position.Occupant = this;
                Thread.Sleep(Delay);
                nextPosition = _route.Next(Position);

                lock (nextPosition)
                {
                    Position.Occupant = default;
                    Position = nextPosition;
                }

                if (_route.IsFinished(Position))
                {
                    lap++;
                }
            }
        } while (lap < rounds && !cancellationToken.IsCancellationRequested);
        
        Position.Occupant = this;
    }
}
