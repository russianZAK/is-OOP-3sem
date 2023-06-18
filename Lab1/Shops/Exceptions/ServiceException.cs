namespace Shops.Exceptions;

public class ServiceException : Exception
{
    private ServiceException(string? message)
        : base(message)
    {
    }

    public static ServiceException UserExistInSystem(string userName)
    {
        throw new ServiceException($"{userName} exists in the system");
    }

    public static ServiceException UserDoesNotExistInSystem(string userName)
    {
        throw new ServiceException($"{userName} doesn't exist in the system");
    }

    public static ServiceException ShopExistInSystem(string shopName)
    {
        throw new ServiceException($"{shopName} exists in the system");
    }

    public static ServiceException ShopDoesNotExistInSystem(string shopName)
    {
        throw new ServiceException($"{shopName} doesn't exist in the system");
    }

    public static ServiceException ProductsDoesNotExistInSystem(string productName)
    {
        throw new ServiceException($"{productName} doesn't exist in the system");
    }

    public static ServiceException NotEnoughAmountOfProducts()
    {
        throw new ServiceException("not enough amount of products");
    }

    public static ServiceException NotEnoughProducts()
    {
        throw new ServiceException("there isn't enough products");
    }
}