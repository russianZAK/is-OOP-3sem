using Shops.Exceptions;

namespace Shops.Models;

public class Cart
{
    private readonly List<ProductInCart> _products;

    public Cart(Guid newGuid)
    {
        _products = new List<ProductInCart>();
        IdGuid = newGuid;
    }

    public Guid IdGuid { get; }

    public IReadOnlyCollection<ProductInCart> Products
    {
        get
        {
            return _products;
        }
    }

    public void AddProduct(Product product, int amount)
    {
        if (product == null) ProductException.InvalidProduct();
        if (amount < 0) ProductException.InvalidAmount(amount);

        var productInCart = new ProductInCart(product!, amount);

        if (_products.Contains(productInCart))
        {
            int newAmountOfItems = _products[_products.IndexOf(productInCart)].Amount + amount;
            _products[_products.IndexOf(productInCart)].SetNewAmount(newAmountOfItems);
        }
        else
        {
            _products.Add(productInCart);
        }
    }

    public void ClearCart()
    {
        _products.Clear();
    }
}
