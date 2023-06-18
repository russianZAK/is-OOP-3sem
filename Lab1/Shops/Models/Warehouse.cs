using Shops.Exceptions;
namespace Shops.Models;

public class Warehouse
{
    private readonly Dictionary<string, Product> _prodcuts;

    public Warehouse()
    {
        _prodcuts = new Dictionary<string, Product>();
    }

    public IReadOnlyCollection<Product> Products
    {
        get
        {
            return _prodcuts.Values.ToList();
        }
    }

    public void AddProduct(Product product)
    {
        if (product == null) ProductException.InvalidProduct();

        if (!_prodcuts.ContainsKey(product!.ProductName))
        {
            _prodcuts.Add(product.ProductName, product);
        }
    }

    public Product? FindProduct(string productName)
    {
        if (productName == null) ProductException.InvalidProductName();

        if (_prodcuts.ContainsKey(productName!))
        {
            return _prodcuts[productName!];
        }

        return null;
    }
}
