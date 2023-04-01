namespace ThreadingLogic.Clients;

public interface IWaitingClientsCounter
{
    int WaitingClients { get; }
}