namespace Shops.Exceptions;

public class ShopException : Exception
{
    private ShopException(string? message)
        : base(message)
    {
    }

    public static ShopException InvalidShopName()
    {
        throw new ShopException("shopname is invalid");
    }

    public static ShopException InvalidShop()
    {
        throw new ShopException("shop is invalid");
    }

    public static ShopException InvalidAddress()
    {
        throw new ShopException("address is invalid");
    }

    public static ShopException InvalidAmountOfMoney(decimal money)
    {
        throw new ShopException($"{money} is invalid");
    }

    public static ShopException ProductDoesNotExistInShop(string? productName)
    {
        throw new ShopException($"{productName} doesn't exist in the shop");
    }

    public static ShopException NotEnoughAmountOfProducts(string shopName)
    {
        throw new ShopException($"{shopName} : there isn't enough amount of products");
    }

    public static ShopException NotEnoughProducts(string shopName)
    {
        throw new ShopException($"{shopName} : there isn't enough products");
    }

    public static ShopException InvalidPriceForItem(decimal price)
    {
        throw new ShopException($"{price} is invalid price");
    }
}
