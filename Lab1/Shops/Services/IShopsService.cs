using Shops.Models;

namespace Shops.Services;

public interface IShopsService
{
    public User AddNewUser(string username, decimal money);
    public Shop AddNewShop(string shopName, string address);
    public Product CreateNewProduct(string productName);
    public void AddProductToCart(string productName, int amountOfProduct, User user);
    public void ChangePriceForItemInShop(Shop shop, Product product, decimal newPrice);
    public Order CreateNewOrder();
    public void AddProductToOrder(Order order, Product product, decimal pricePerItem, int amount);
    public void DeliverItemsToShop(Shop shop, Order order);
    public Shop SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible(User user);
    public void BuyProducts(User user, Shop shop);
    public int GetProductInfo(Shop shop, Product product);
    public Shop GetShop(string shopName);
    public decimal GetProductPrice(Shop shop, Product product);
}