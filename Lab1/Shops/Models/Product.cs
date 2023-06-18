using Shops.Exceptions;

namespace Shops.Models;

public class Product
{
    public Product(string productName, Guid newGuid)
    {
        if (productName == null) ProductException.InvalidProductName();

        ProductName = productName!;
        Id = newGuid;
    }

    public string ProductName { get; }
    public Guid Id { get; }
}
