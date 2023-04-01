namespace ThreadingLogic.Map;

public class GoCart : IRouteAccessor, IOccupant
{
    private readonly IRoute<Section> _raceRoute;
    public string Marker { get; set; } = String.Empty;
    public string HexColor { get; }
    private Section? Position { get; set; }
    
    public int Delay { get; set; }
    
    public GoCart(IRoute<Section> raceRoute, string hexColor)
    {
        _raceRoute = raceRoute;
        HexColor = hexColor;
    }
    
    public void DoRounds(int rounds, CancellationToken cancellationToken)
    {
        if (rounds < 1)
        {
            return;
        }
        
        int lap = 0;
        Section nextPosition = _raceRoute.Enter();
        Position = _raceRoute.Enter();
        
        do
        {
            lock (Position)
            {
                Position.Occupant = this;
                Thread.Sleep(Delay);
                nextPosition = _raceRoute.Next(Position);

                lock (nextPosition)
                {
                    Position.Occupant = default;
                    Position = nextPosition;
                }

                if (_raceRoute.IsFinished(Position))
                {
                    lap++;
                }
            }
        } while (lap < rounds && !cancellationToken.IsCancellationRequested);
        
        Position.Occupant = this;
    }
}
