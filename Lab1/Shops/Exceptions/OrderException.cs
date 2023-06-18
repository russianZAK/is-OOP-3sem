namespace Shops.Exceptions;

public class OrderException : Exception
{
    private OrderException(string? message)
        : base(message)
    {
    }

    public static OrderException InvalidPricePerItem(decimal price)
    {
        throw new OrderException($"{price} is invalid");
    }

    public static OrderException InvalidAmount(int amount)
    {
        throw new OrderException($"{amount} is invalid");
    }

    public static OrderException InvalidOrder()
    {
        throw new OrderException("order is invalid");
    }
}