using Shops.Exceptions;

namespace Shops.Models;

public class User
{
    private readonly Dictionary<string, PurchasedUsersProduct> items;
    public User(string username, decimal money, int id)
    {
        if (username == null) UserException.InvalidUserName();
        if (money < 0) UserException.InvalidAmountOfMoney(money);

        Username = username!;
        Money = money;

        items = new Dictionary<string, PurchasedUsersProduct>();
        Id = id;
    }

    public IReadOnlyCollection<PurchasedUsersProduct> Items
    {
        get
        {
            return items.Values.ToList();
        }
    }

    public string Username { get; }
    public int Id { get; }
    public decimal Money { get; private set; }

    public void ChangeMoney(decimal newMoney)
    {
        if (newMoney < 0) UserException.InvalidAmountOfMoney(newMoney);
        Money = newMoney;
    }

    public void AddProduct(ProductInCart productInCart)
    {
        if (productInCart == null) ProductInCartException.InvalidProductInCart();

        if (items.ContainsKey(productInCart!.Product.ProductName))
        {
            items[productInCart.Product.ProductName].ChangeAmount(items[productInCart.Product.ProductName].Amount + productInCart.Amount);
        }
        else
        {
            items.Add(productInCart.Product.ProductName, new PurchasedUsersProduct(productInCart.Amount, productInCart.Product));
        }
    }
}