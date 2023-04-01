using ThreadingLogic.Map;

namespace ThreadingLogic.Buffers;

public interface IRouteAccessorsBuffer<T>
    where T : IRouteAccessor
{
    T? Get();
    void Put(T goCart);

    IReadOnlyList<string> Preview();
}