namespace WebUtilities.Interfaces;

public interface IDataSource<T> : IDisposable
{
    public T Source { get; set; }
}