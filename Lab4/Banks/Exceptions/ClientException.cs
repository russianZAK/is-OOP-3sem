namespace Banks.Exceptions;

public class ClientException : Exception
{
    private ClientException(string? message)
   : base(message)
    { }

    public static ClientException InvalidBankAccount()
    {
        return new ClientException("Bank account is invalid");
    }

    public static ClientException InvalidBank()
    {
        return new ClientException("Bank is invalid");
    }

    public static ClientException InvalidPayload()
    {
        return new ClientException("payload is invalid");
    }

    public static ClientException InvalidData()
    {
        return new ClientException("data is invalid");
    }

    public static ClientException InvalidMediator()
    {
        return new ClientException("mediator is invalid");
    }
}
