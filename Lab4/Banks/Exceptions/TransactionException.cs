namespace Banks.Exceptions;

public class TransactionException : Exception
{
    private TransactionException(string? message)
 : base(message)
    { }

    public static TransactionException InvalidBankAccount()
    {
        return new TransactionException("bank account is invalid");
    }

    public static TransactionException InvalidMoney(decimal money)
    {
        return new TransactionException($"{money} is invalid");
    }

    public static TransactionException InvalidTransaction()
    {
        return new TransactionException("transaction is invalid");
    }
}
