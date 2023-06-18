namespace Banks.Exceptions;

public class CentralBankException : Exception
{
    private CentralBankException(string? message)
    : base(message)
    { }

    public static CentralBankException InvalidWatcher()
    {
        return new CentralBankException("wathcer is invalid");
    }
}