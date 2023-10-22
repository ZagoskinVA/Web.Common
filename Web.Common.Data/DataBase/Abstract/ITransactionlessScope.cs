using WebUtilities.Interfaces;

namespace Web.Common.Data.DataBase.Abstract;

public interface ITransactionlessScope: IDataContext
{
    public IContext Context { get; }
}