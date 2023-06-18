using Shops.Exceptions;

namespace Shops.Models;

public class ProductInCart
{
    public ProductInCart(Product product, int amount)
    {
        if (product == null) ProductException.InvalidProduct();
        if (amount < 0) ProductException.InvalidAmount(amount);

        Product = product!;
        Amount = amount;
    }

    public Product Product { get; }
    public int Amount { get; private set; }

    public void SetNewAmount(int newAmount)
    {
        if (newAmount < 0) ProductException.InvalidAmount(newAmount);
        Amount = newAmount;
    }
}
