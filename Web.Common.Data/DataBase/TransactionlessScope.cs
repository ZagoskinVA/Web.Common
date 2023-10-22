using Web.Common.Data.DataBase.Abstract;
using WebUtilities.Interfaces;

namespace Web.Common.Data.DataBase;

public class TransactionlessScope: ITransactionlessScope
{
    public IContext Context { get; }

    public TransactionlessScope(IContext context)
    {
        Context = context;
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}