using System;

namespace ThreadingLogic.Map;

public class RouteAccessor
{
    private readonly Route _route;
    private readonly string _id;
    public Section? CurrentPosition { get; private set; }
    
    public RouteAccessor(Route route, string id)
    {
        _route = route;
        _id = id;
    }

    public void Enter()
    {
        CurrentPosition = _route.Enter();
        CurrentPosition.Occupant = _id;
    }
    
    public void MoveForward()
    {
        var nextPosition = _route.Next(CurrentPosition!);
        CurrentPosition!.Occupant = String.Empty;
    }

    public void Leave()
    {
        if (CurrentPosition is null)
        {
            throw new InvalidOperationException();
        }
        
        CurrentPosition.Occupant = String.Empty;
        CurrentPosition = null;
    }
}