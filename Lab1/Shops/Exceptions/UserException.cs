using Shops.Models;

namespace Shops.Exceptions;

public class UserException : Exception
{
    private UserException(string? message)
        : base(message)
    {
    }

    public static UserException InvalidUserName()
    {
        throw new UserException("username is invalid");
    }

    public static UserException NotEnoughMoney(User user)
    {
        throw new UserException($"{user.Username} hasn't enough money to buy products");
    }

    public static UserException InvalidAmountOfMoney(decimal money)
    {
        throw new UserException($"{money} is invalid");
    }

    public static UserException InvalidUser()
    {
        throw new UserException("user is invalid");
    }
}