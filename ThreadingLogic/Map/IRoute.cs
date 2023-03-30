namespace ThreadingLogic.Map;

public interface IRoute
{
    IReadOnlyList<Section> Map { get; }
}