using Shops.Exceptions;
using Shops.Models;

namespace Shops.Services;

public class ShopsService : IShopsService
{
    private readonly Dictionary<string, Shop> _shops;
    private readonly Dictionary<string, KeyValuePair<User, Cart>> _users;
    private readonly Warehouse _warehouse;
    private int shopIds = 0;
    private int usersIds = 0;
    private int orderIds = 0;

    public ShopsService()
    {
        _shops = new Dictionary<string, Shop>();
        _users = new Dictionary<string, KeyValuePair<User, Cart>>();
        _warehouse = new Warehouse();
    }

    public IReadOnlyCollection<Shop> Shops
    {
        get
        {
            return _shops.Values.ToList();
        }
    }

    public IReadOnlyDictionary<User, Cart> Users
    {
        get
        {
            return _users.ToDictionary(pair => pair.Value.Key, pair => pair.Value.Value);
        }
    }

    public User AddNewUser(string username, decimal money)
    {
        if (username == null) UserException.InvalidUserName();
        if (money < 0) UserException.InvalidAmountOfMoney(money);
        if (_users.ContainsKey(username!)) ServiceException.UserExistInSystem(username!);

        var newUser = new User(username!, money, usersIds);
        usersIds++;
        AddUser(newUser, new Cart(Guid.NewGuid()));
        return newUser;
    }

    public Shop AddNewShop(string shopName, string address)
    {
        if (shopName == null) ShopException.InvalidShopName();
        if (address == null) ShopException.InvalidAddress();
        if (_shops.ContainsKey(shopName!)) ServiceException.ShopExistInSystem(shopName!);

        var newShop = new Shop(shopName!, address!, shopIds);
        shopIds++;
        AddShop(shopName!, newShop);
        return newShop;
    }

    public Product CreateNewProduct(string productName)
    {
        CheckProductName(productName);
        var newProduct = new Product(productName, Guid.NewGuid());
        _warehouse.AddProduct(newProduct);
        return newProduct;
    }

    public void AddProductToCart(string productName, int amountOfProduct, User user)
    {
        CheckNullUser(user);
        if (!_users.ContainsKey(user!.Username)) ServiceException.UserDoesNotExistInSystem(user.Username);
        CheckProductName(productName);
        CheckAmountOfProduct(amountOfProduct);

        Product? product = _warehouse.FindProduct(productName);
        if (product == null) ServiceException.ProductsDoesNotExistInSystem(productName);

        _users[user.Username].Value.AddProduct(product!, amountOfProduct);
    }

    public void ChangePriceForItemInShop(Shop shop, Product product, decimal newPrice)
    {
        CheckNullShop(shop);
        CheckNullProduct(product);
        if (newPrice < 0) ShopException.InvalidPriceForItem(newPrice);
        if (!_shops.ContainsKey(shop!.ShopName)) ServiceException.ShopDoesNotExistInSystem(shop.ShopName);

        IEnumerable<Product> checkingProducts = _shops[shop.ShopName].Items.Where(batchOfGoods => batchOfGoods.Product == product).Select(batchOfGoods => batchOfGoods.Product);

        if (checkingProducts.Count() == 0) ShopException.ProductDoesNotExistInShop(product.ProductName);

        _shops[shop.ShopName].ChangePricePerItem(product, newPrice);
    }

    public Order CreateNewOrder()
    {
        var newOrder = new Order(orderIds);

        orderIds++;
        return newOrder;
    }

    public void AddProductToOrder(Order order, Product product, decimal pricePerItem, int amount)
    {
        CheckNullOrder(order);
        CheckNullProduct(product);
        CheckAmountOfProduct(amount);
        if (pricePerItem < 0) OrderException.InvalidPricePerItem(pricePerItem);

        order.AddProduct(product, pricePerItem, amount, Guid.NewGuid());
    }

    public void DeliverItemsToShop(Shop shop, Order order)
    {
        CheckNullShop(shop);
        CheckNullOrder(order);
        if (!_shops.ContainsKey(shop.ShopName)) ServiceException.ShopDoesNotExistInSystem(shop.ShopName);
        var productsWhichExistInShop = order.Items.Where(batchOfGoods => _shops[shop.ShopName].Items.Contains(batchOfGoods)).ToList();
        var productsWhichDoestNotExistInShop = order.Items.Where(batchOfGoods => (!_shops[shop.ShopName].Items.Contains(batchOfGoods))).ToList();

        productsWhichDoestNotExistInShop.ForEach(batchOfGoods => AddProductToShop(shop, batchOfGoods));
        productsWhichExistInShop.ForEach(batchOfGoods => AddExistingProductToShop(shop, batchOfGoods));
    }

    public Shop SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible(User user)
    {
        CheckNullUser(user);

        IEnumerable<Shop> shopsWithAllProducts = _shops.Values.Where(shop => _users[user.Username].Value.Products.ToList().All(product => shop.Items.ToDictionary(productName => productName.Product.ProductName, productInShop => productInShop.Product).ContainsKey(product.Product.ProductName))).Select(shop => shop);
        if (shopsWithAllProducts.Count() == 0) ServiceException.NotEnoughProducts();

        var shopsWithoutAllEnoughProducts = new List<Shop>();

        foreach (Shop shopWithAllProduct in shopsWithAllProducts)
        {
            foreach (ProductInCart productInCart in _users[user.Username].Value.Products)
            {
                var tempDictionaryForCheckingProducts = _shops[shopWithAllProduct.ShopName].Items.ToDictionary(batchOfGoods => batchOfGoods.Product.ProductName, batchOfGoods => batchOfGoods);
                if (productInCart.Amount > tempDictionaryForCheckingProducts[productInCart.Product.ProductName].Amount)
                {
                    shopsWithoutAllEnoughProducts.Add(shopWithAllProduct);
                }
            }
        }

        IEnumerable<Shop> uniqShopsWithoutAllEnoughProducts = shopsWithoutAllEnoughProducts.Distinct();

        if (uniqShopsWithoutAllEnoughProducts.Count() == shopsWithAllProducts.Count()) ServiceException.NotEnoughAmountOfProducts();
        IEnumerable<Shop> shopsWithAllEnoughProducts = shopsWithAllProducts.Where(shop => (!uniqShopsWithoutAllEnoughProducts.Contains(shop))).Select(shop => shop);

        var shopsAndPrices = new Dictionary<Shop, decimal>();

        foreach (Shop shop in shopsWithAllEnoughProducts)
        {
            decimal allSum = 0;

            foreach (ProductInCart productInCart in _users[user.Username].Value.Products)
            {
                var tempDictionaryForProducts = shop.Items.ToDictionary(batchOfGoods => batchOfGoods.Product.ProductName, batchOfGoods => batchOfGoods);

                allSum += tempDictionaryForProducts[productInCart.Product.ProductName].PricePerItem * productInCart.Amount;
            }

            shopsAndPrices.Add(shop, allSum);
        }

        var sortedShops = shopsAndPrices.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        return sortedShops.First().Key;
    }

    public void BuyProducts(User user, Shop shop)
    {
        CheckNullUser(user);
        CheckNullShop(shop);
        if (!_shops.ContainsKey(shop.ShopName)) ServiceException.ShopDoesNotExistInSystem(shop.ShopName);
        if (!_users.ContainsKey(user.Username)) ServiceException.UserDoesNotExistInSystem(user.Username);

        if (!_users[user.Username].Value.Products.ToList().All(product => _shops[shop.ShopName].Items.ToDictionary(productName => productName.Product.ProductName, productInShop => productInShop.Product).ContainsKey(product.Product.ProductName))) ShopException.NotEnoughProducts(shop.ShopName);

        IEnumerable<Product> shopProducts = _shops[shop.ShopName].Items.Select(batchOfGoods => batchOfGoods.Product);

        foreach (ProductInCart productUserInCart in _users[user.Username].Value.Products)
        {
            if (!shopProducts.Contains(productUserInCart.Product))
            {
                ShopException.NotEnoughProducts(shop.ShopName);
            }
        }

        IEnumerable<Product> productsWithoutEnoughAmount = _users[user.Username].Value.Products.Where(productIncart => productIncart.Amount > _shops[shop.ShopName].Items.ToDictionary(batchOfGoods => batchOfGoods.Product.ProductName, batchOfGoods => batchOfGoods)[productIncart.Product.ProductName].Amount).Select(productInCart => productInCart.Product);

        var uniqShopsWithoutAllEnoughProducts = productsWithoutEnoughAmount.Distinct().ToList();
        if (uniqShopsWithoutAllEnoughProducts.Count > 0) ShopException.NotEnoughAmountOfProducts(shop.ShopName);

        decimal summOfPurchasing = 0;

        var products = _users[user.Username].Value.Products.ToList();

        products.ForEach(product =>
        {
            var tempDictionaryForProducts = _shops[shop.ShopName].Items.ToDictionary(batchOfGoods => batchOfGoods.Product.ProductName, batchOfGoods => batchOfGoods);
            int indexOfProduct = _users[user.Username].Value.Products.ToList().IndexOf(product);
            summOfPurchasing += tempDictionaryForProducts[product.Product.ProductName].PricePerItem * _users[user.Username].Value.Products.ToList()[indexOfProduct].Amount;
        });
        if (_users[user.Username].Key.Money < summOfPurchasing) UserException.NotEnoughMoney(user);

        decimal newUsersMoney = _users[user.Username].Key.Money - summOfPurchasing;
        _users[user.Username].Key.ChangeMoney(newUsersMoney);
        decimal newShopMoney = _shops[shop.ShopName].Money + summOfPurchasing;
        _shops[shop.ShopName].ChangeMoney(newShopMoney);

        products.ForEach(product =>
        {
            var tempDictionaryForProducts = _shops[shop.ShopName].Items.ToDictionary(batchOfGoods => batchOfGoods.Product.ProductName, batchOfGoods => batchOfGoods);

            _users[user.Username].Key.AddProduct(product);
            int newAmountOfItems = tempDictionaryForProducts[product.Product.ProductName].Amount - _users[user.Username].Value.Products.ToList()[_users[user.Username].Value.Products.ToList().IndexOf(product)].Amount;

            foreach (BatchOfGoods batchOfGoods in _shops[shop.ShopName].Items)
            {
                if (batchOfGoods.Product.ProductName == product.Product.ProductName) batchOfGoods.ChangeAmountOfItem(newAmountOfItems);
            }
        });
        _users[user.Username].Value.ClearCart();
    }

    public int GetProductInfo(Shop shop, Product product)
    {
        CheckNullProduct(product);
        CheckNullShop(shop);
        if (!_shops.ContainsKey(shop.ShopName)) ServiceException.ShopDoesNotExistInSystem(shop.ShopName);

        IEnumerable<Product> checkingProducts = _shops[shop.ShopName].Items.Where(batchOfGoods => batchOfGoods.Product == product).Select(batchOfGoods => batchOfGoods.Product);

        if (checkingProducts.Count() == 0) ShopException.ProductDoesNotExistInShop(product.ProductName);

        var tempDictionaryForProducts = _shops[shop.ShopName].Items.ToDictionary(batchOfGoods => batchOfGoods.Product.ProductName, batchOfGoods => batchOfGoods);

        return tempDictionaryForProducts[product.ProductName].Amount;
    }

    public Shop GetShop(string shopName)
    {
        if (shopName == null) ShopException.InvalidShopName();

        if (!_shops.ContainsKey(shopName!)) ServiceException.ProductsDoesNotExistInSystem(shopName!);

        return _shops[shopName!];
    }

    public decimal GetProductPrice(Shop shop, Product product)
    {
        CheckNullProduct(product);
        CheckNullShop(shop);
        if (!_shops.ContainsKey(shop.ShopName)) ServiceException.ShopDoesNotExistInSystem(shop.ShopName);

        IEnumerable<Product> checkingProducts = _shops[shop.ShopName].Items.Where(batchOfGoods => batchOfGoods.Product == product).Select(batchOfGoods => batchOfGoods.Product);

        if (checkingProducts.Count() == 0) ShopException.ProductDoesNotExistInShop(product.ProductName);

        var tempDictionaryForProducts = _shops[shop.ShopName].Items.ToDictionary(batchOfGoods => batchOfGoods.Product.ProductName, batchOfGoods => batchOfGoods);

        return tempDictionaryForProducts[product.ProductName].PricePerItem;
    }

    private void CheckProductName(string productName)
    {
        if (productName == null) ProductException.InvalidProductName();
    }

    private void CheckNullUser(User user)
    {
        if (user == null) UserException.InvalidUser();
    }

    private void CheckNullShop(Shop shop)
    {
        if (shop == null) ShopException.InvalidShop();
    }

    private void CheckNullOrder(Order order)
    {
        if (order == null) OrderException.InvalidOrder();
    }

    private void CheckNullProduct(Product product)
    {
        if (product == null) ProductException.InvalidProduct();
    }

    private void CheckAmountOfProduct(int amountOfProduct)
    {
        if (amountOfProduct < 0) ProductException.InvalidAmount(amountOfProduct);
    }

    private void AddProductToShop(Shop shop, BatchOfGoods batchOfGoods)
    {
        _shops[shop.ShopName].AddProducts(batchOfGoods);
    }

    private void AddShop(string shopName, Shop newShop)
    {
        _shops.Add(shopName, newShop);
    }

    private void AddUser(User user, Cart cart)
    {
        _users.Add(user.Username, new KeyValuePair<User, Cart>(user, cart));
    }

    private void AddExistingProductToShop(Shop shop, BatchOfGoods batchOfGoods)
    {
        var tempDictionaryForProducts = _shops[shop.ShopName].Items.ToDictionary(batchOfGoods => batchOfGoods.Product.ProductName, batchOfGoods => batchOfGoods);

        int newAmountOfItems = tempDictionaryForProducts[batchOfGoods.Product.ProductName].Amount + batchOfGoods.Amount;
        tempDictionaryForProducts[batchOfGoods.Product.ProductName].ChangeAmountOfItem(newAmountOfItems);
    }
}
