using Shops.Exceptions;

namespace Shops.Models;

public class Shop
{
    private readonly Dictionary<string, BatchOfGoods> items;
    public Shop(string shopName, string address, int id)
    {
        if (shopName == null) ShopException.InvalidShopName();
        if (address == null) ShopException.InvalidAddress();

        ShopName = shopName!;
        Address = address!;
        Money = 0;
        items = new Dictionary<string, BatchOfGoods>();
        Id = id;
    }

    public IReadOnlyCollection<BatchOfGoods> Items
    {
        get
        {
            return items.Values.ToList();
        }
    }

    public int Id { get; }
    public string ShopName { get; }
    public string Address { get; }
    public decimal Money { get; private set; }

    public void ChangeMoney(decimal newMoney)
    {
        if (newMoney < 0) ShopException.InvalidAmountOfMoney(newMoney);
        Money = newMoney;
    }

    public void AddProducts(BatchOfGoods batchOfGoods)
    {
        if (batchOfGoods == null) BatchOfGoodsException.InvalidBatchOfGoods();

        if (items.ContainsKey(batchOfGoods!.Product.ProductName))
        {
            int newAmountOfItems = items[batchOfGoods.Product.ProductName].Amount + batchOfGoods.Amount;
            items[batchOfGoods.Product.ProductName].ChangeAmountOfItem(newAmountOfItems);
        }
        else
        {
            items.Add(batchOfGoods.Product.ProductName, batchOfGoods);
        }
    }

    public void ChangePricePerItem(Product product, decimal newPrice)
    {
        if (product == null) ProductException.InvalidProduct();
        if (newPrice < 0) ShopException.InvalidPriceForItem(newPrice);
        if (!items.ContainsKey(product!.ProductName)) ShopException.ProductDoesNotExistInShop(product.ProductName);

        items[product!.ProductName].ChangePricePerItem(newPrice);
    }
}
