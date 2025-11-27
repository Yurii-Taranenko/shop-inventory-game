/// <summary>
/// Minimal abstraction for saving/loading a single data object.
/// </summary>
public interface IRepo<T>
{
    T Load();
    void Save(T data);
}