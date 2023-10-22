using Microsoft.EntityFrameworkCore;

namespace WebUtilities.Interfaces;

public interface IDataContext: IDisposable
{
    IContext Context { get; }
}