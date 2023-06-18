using Shops.Exceptions;
namespace Shops.Models;

public class PurchasedUsersProduct
{
    public PurchasedUsersProduct(int amount, Product product)
    {
        if (amount < 0) ProductException.InvalidAmount(amount);
        if (product == null) ProductException.InvalidProduct();

        Product = product!;
        Amount = amount;
    }

    public int Amount { get; private set; }
    public Product Product { get; }

    public void ChangeAmount(int newAmount)
    {
        if (newAmount < 0) ProductException.InvalidAmount(newAmount);
        Amount = newAmount;
    }
}
