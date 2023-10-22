using Microsoft.EntityFrameworkCore;

namespace WebUtilities.Interfaces;

public interface ITransactionContext: IDataContext
{
    IContext Context { get; }

    void OnCommittedAsync(Func<Task> callback);
}