using Microsoft.EntityFrameworkCore.Storage;
using Web.Common.Data.DataBase.Abstract;
using WebUtilities.Interfaces;

namespace Web.Common.Data.DataBase;

public class TransactionScope: ITransactionScope
{
    public IContext Context { get; }
    public Func<Task>? afterCommitedFunc;


    public void OnCommittedAsync(Func<Task> callback)
    {
        afterCommitedFunc = callback;
    }

    private readonly IDbContextTransaction _contextTransaction;
    
    public TransactionScope(ApplicationContext context)
    {
        Context = context;
        _contextTransaction = context.Database.BeginTransaction();
    }
    public async Task CommitAsync()
    {
        await _contextTransaction.CommitAsync();

        if (afterCommitedFunc != null)
        {
            await afterCommitedFunc();
        }

    }

    public async Task RollbackAsync()
    {
        await _contextTransaction.RollbackAsync();
    }

    public void Dispose()
    {
        _contextTransaction.Dispose();
        Context.Dispose();
    }
}