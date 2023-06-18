namespace Shops.Exceptions;

public class ProductInCartException : Exception
{
    private ProductInCartException(string? message)
        : base(message)
    {
    }

    public static ProductInCartException InvalidProductInCart()
    {
        return new ProductInCartException("product in cart is invalid");
    }
}
