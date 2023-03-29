namespace ThreadingLogic.Buffers;

public interface IBuffer<T>
{
    T? Get();
    void Put(T goCart);
}