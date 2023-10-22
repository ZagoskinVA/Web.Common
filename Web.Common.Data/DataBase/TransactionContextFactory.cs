using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Web.Common.Data.DataBase.Abstract;

namespace Web.Common.Data.DataBase;

public class TransactionContextFactory: ITransactionContextFactory
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public TransactionContextFactory(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    public ITransactionlessScope CreateTransactionLess()
    {
        return new TransactionlessScope(_contextFactory.CreateDbContext());
    }

    public ITransactionScope Create()
    {
        var context = _contextFactory.CreateDbContext();
        return new TransactionScope(context);
    }
}