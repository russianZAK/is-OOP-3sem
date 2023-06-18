namespace Banks.Exceptions;

public class BankAccountIdException : Exception
{
    private BankAccountIdException(string? message)
   : base(message)
    { }

    public static BankAccountIdException InvalidId(int id)
    {
        return new BankAccountIdException($"{id} is invalid");
    }
}