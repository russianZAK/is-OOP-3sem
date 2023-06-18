namespace Shops.Exceptions;

public class ProductException : Exception
{
    private ProductException(string? message)
        : base(message)
    {
    }

    public static ProductException InvalidProductName()
    {
         return new ProductException("productname is invalid");
    }

    public static ProductException InvalidAmount(int amount)
    {
        return new ProductException($"{amount} is invalid");
    }

    public static ProductException InvalidProduct()
    {
        return new ProductException("product is invalid");
    }
}
