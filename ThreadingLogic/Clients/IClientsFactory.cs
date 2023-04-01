using ThreadingLogic.Map;

namespace ThreadingLogic.Clients;

public interface IClientsFactory<T>
    where T : IRouteAccessor
{
    Client<T> Create(Action onEnter, CancellationToken cancellationToken);
}