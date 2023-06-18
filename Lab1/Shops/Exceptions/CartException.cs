namespace Shops.Exceptions;

public class CartException : Exception
{
    private CartException(string? message)
        : base(message)
    {
    }

    public static CartException InvalidCart()
    {
        throw new CartException("cart is invalid");
    }
}