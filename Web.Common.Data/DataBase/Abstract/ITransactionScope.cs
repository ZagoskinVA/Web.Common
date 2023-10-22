using WebUtilities.Interfaces;

namespace Web.Common.Data.DataBase.Abstract;

public interface ITransactionScope: ITransactionContext
{
    IContext Context { get; }
    Task CommitAsync();
    Task RollbackAsync();
}