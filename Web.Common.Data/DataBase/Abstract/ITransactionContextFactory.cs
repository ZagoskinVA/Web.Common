namespace Web.Common.Data.DataBase.Abstract;

public interface ITransactionContextFactory
{
    ITransactionlessScope CreateTransactionLess();
    ITransactionScope Create();
}