using Shops.Exceptions;

namespace Shops.Models;

public class Order
{
    private readonly Dictionary<string, BatchOfGoods> items;
    public Order(int id)
    {
        Id = id;
        items = new Dictionary<string, BatchOfGoods>();
    }

    public int Id { get; }
    public IReadOnlyCollection<BatchOfGoods> Items
    {
        get
        {
            return items.Values.ToList();
        }
    }

    public void AddProduct(Product product, decimal pricePerItem, int amount, Guid newGuid)
    {
        if (product == null) ProductException.InvalidProduct();
        if (pricePerItem < 0) OrderException.InvalidPricePerItem(pricePerItem);
        if (amount < 0) OrderException.InvalidAmount(amount);

        if (items.ContainsKey(product!.ProductName))
        {
            decimal newAveragePrice = Math.Round((items[product.ProductName].Amount * items[product.ProductName].PricePerItem) + (pricePerItem * amount)) / (items[product.ProductName].Amount + amount);
            int newAmountOfItems = items[product.ProductName].Amount + amount;
            items[product.ProductName].ChangeAmountOfItem(newAmountOfItems);
            items[product.ProductName].ChangePricePerItem(newAveragePrice);
        }
        else
        {
            items.Add(product.ProductName, new BatchOfGoods(product, pricePerItem, amount, newGuid));
        }
    }
}
