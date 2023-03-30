namespace ThreadingLogic.Map;

public interface IRouteAccessor
{
    void DoRounds(int rounds, CancellationToken cancellationToken);
    string Marker { get; set; }
    int Delay { get; set; }
}