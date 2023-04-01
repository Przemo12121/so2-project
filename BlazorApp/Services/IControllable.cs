namespace BlazorApp.Services;

public interface IControllable
{
    bool IsRunning { get; }
    void Start();
    void Stop();

    ThreadingLogicStatus Status();
}