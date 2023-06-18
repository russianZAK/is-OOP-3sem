using Shops.Exceptions;
namespace Shops.Models;

public class BatchOfGoods
{
    public BatchOfGoods(Product product, decimal pricePerItem, int amount, Guid newGuid)
    {
        if (product == null)
        {
            ProductException.InvalidProduct();
        }

        if (pricePerItem < 0)
        {
            BatchOfGoodsException.InvalidPricePerItem(pricePerItem);
        }

        if (amount < 0)
        {
            BatchOfGoodsException.InvalidAmount(amount);
        }

        Product = product!;
        PricePerItem = pricePerItem;
        Amount = amount;
        IdGuid = newGuid;
    }

    public Guid IdGuid { get; }

    public Product Product { get; }

    public decimal PricePerItem { get; private set; }

    public int Amount { get; private set; }

    public void ChangeAmountOfItem(int newAmount)
    {
        if (newAmount < 0)
        {
            BatchOfGoodsException.InvalidAmount(newAmount);
        }

        Amount = newAmount;
    }

    public void ChangePricePerItem(decimal newPrice)
    {
        if (newPrice < 0)
        {
            BatchOfGoodsException.InvalidPricePerItem(newPrice);
        }

        PricePerItem = newPrice;
    }
}
