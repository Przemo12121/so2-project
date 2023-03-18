namespace ThreadingLogic.Map;

public class GoCart : IRouteAccessor
{
    private readonly Route _route;
    private readonly string _id;
    private Section? Position { get; set; }
    
    public int Delay { get; set; }
    
    public GoCart(Route route, string id)
    {
        _route = route;
        _id = id;
    }
    
    public void DoRounds(int rounds)
    {
        if (rounds < 1)
        {
            return;
        }
        Section nextPosition;

        for (int lap = 0; lap < rounds; lap++)
        {
            Position = _route.Enter();
            
            while (_route.IsFinished(Position) is false)
            {
                lock (Position)
                {
                    Position.Occupant = _id;
                    Thread.Sleep(Delay);
                    nextPosition = _route.Next(Position);

                    lock (nextPosition)
                    {
                        nextPosition.Occupant = _id;
                        Position!.Occupant = String.Empty;
                        Position = nextPosition;
                    }
                }
            }

            lock (Position)
            {
                Position.Occupant = _id;
                Thread.Sleep(Delay);
                Position.Occupant = String.Empty;
            }
        }
        
    }
}
