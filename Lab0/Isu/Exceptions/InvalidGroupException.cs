namespace Isu.Exceptions;

public class InvalidGroupException : Exception
{
    public InvalidGroupException(string? textMessage)
    {
        TextMessage = textMessage;
    }

    public string? TextMessage { get; set; }
}
