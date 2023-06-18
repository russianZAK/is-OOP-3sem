namespace Banks.Exceptions;

public class AggregatorException : Exception
{
    private AggregatorException(string? message)
   : base(message)
    { }

    public static AggregatorException InvalidPayload()
    {
        return new AggregatorException("payload is invalid");
    }

    public static AggregatorException InvalidObserver()
    {
        return new AggregatorException("observer is invalid");
    }
}