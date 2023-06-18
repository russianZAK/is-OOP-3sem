using Shops.Exceptions;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class ShopServiceTests
{
    private readonly ShopsService service;

    public ShopServiceTests()
    {
        service = new ShopsService();
    }

    [Fact]
    public void DeliverItemsToShop()
    {
        var allProducts = new List<Product>();

        Shop apple = service.AddNewShop("Apple", "California, USA");

        Product iphone14 = service.CreateNewProduct("Iphone14");
        Product iphone13 = service.CreateNewProduct("Iphone13");
        Product iphone12 = service.CreateNewProduct("Iphone12");

        allProducts.Add(iphone14);
        allProducts.Add(iphone13);
        allProducts.Add(iphone12);

        Order orderToApple = service.CreateNewOrder();
        service.AddProductToOrder(orderToApple, iphone14, 140000, 10);
        service.AddProductToOrder(orderToApple, iphone13, 120000, 5);
        service.AddProductToOrder(orderToApple, iphone12, 100000, 7);

        service.DeliverItemsToShop(apple, orderToApple);

        var allProductsFromShop = new List<Product>();

        Shop appleShop = service.GetShop("Apple");

        foreach (BatchOfGoods batchOfGoods in appleShop.Items)
        {
            allProductsFromShop.Add(batchOfGoods.Product);
        }

        Assert.Equal(allProductsFromShop, allProducts);
    }

    [Fact]
    public void ChangePriceForProduct()
    {
        decimal newPrice = 100000;

        Shop apple = service.AddNewShop("Apple", "California, USA");
        Product iphone14 = service.CreateNewProduct("Iphone14");
        Order orderToApple = service.CreateNewOrder();

        service.AddProductToOrder(orderToApple, iphone14, 140000, 10);
        service.DeliverItemsToShop(apple, orderToApple);
        service.ChangePriceForItemInShop(apple, iphone14, newPrice);

        Assert.Equal(service.GetProductPrice(apple, iphone14), newPrice);
    }

    [Fact]
    public void ChangePriceForProduct_InvalidPrice()
    {
        decimal newPrice = -100000;

        Shop apple = service.AddNewShop("Apple", "California, USA");
        Product iphone14 = service.CreateNewProduct("Iphone14");
        Order orderToApple = service.CreateNewOrder();

        service.AddProductToOrder(orderToApple, iphone14, 140000, 10);
        service.DeliverItemsToShop(apple, orderToApple);

        Assert.Throws<ShopException>(() => service.ChangePriceForItemInShop(apple, iphone14, newPrice));
    }

    [Fact]
    public void SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible()
    {
        User nikita = service.AddNewUser("Nikita", 1000000);

        Shop apple = service.AddNewShop("Apple", "California, USA");
        Shop reStore = service.AddNewShop("reStore", "St.Perersburg, Russia");

        Product product1 = service.CreateNewProduct("Iphone14");
        Product product2 = service.CreateNewProduct("Iphone13");
        Product product3 = service.CreateNewProduct("Iphone12");

        Product iphone14 = service.CreateNewProduct("Iphone14");
        Product iphone13 = service.CreateNewProduct("Iphone13");
        Product iphone12 = service.CreateNewProduct("Iphone12");

        Order order = service.CreateNewOrder();
        Order order2 = service.CreateNewOrder();

        service.AddProductToOrder(order, product1, 140000, 10);
        service.AddProductToOrder(order, product2, 120000, 5);
        service.AddProductToOrder(order, product3, 100000, 7);

        service.DeliverItemsToShop(apple, order);

        service.AddProductToOrder(order2, iphone14, 100000, 10);
        service.AddProductToOrder(order2, iphone13, 90000, 5);
        service.AddProductToOrder(order2, iphone12, 70000, 7);

        service.DeliverItemsToShop(reStore, order2);

        service.AddProductToCart("Iphone14", 1, nikita);
        service.AddProductToCart("Iphone13", 4, nikita);

        Shop bestPriceShop = service.SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible(nikita);
        Assert.Equal(bestPriceShop, reStore);

        service.ChangePriceForItemInShop(reStore, iphone14, 180000);
        service.ChangePriceForItemInShop(reStore, iphone13, 170000);
        service.ChangePriceForItemInShop(reStore, iphone12, 160000);

        Shop bestPriceShop2 = service.SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible(nikita);
        Assert.Equal(bestPriceShop2, apple);
    }

    [Fact]
    public void SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible_NotEnoughAmountOfProducts()
    {
        User nikita = service.AddNewUser("Nikita", 1000000);

        Shop apple = service.AddNewShop("Apple", "California, USA");
        Shop reStore = service.AddNewShop("reStore", "St.Perersburg, Russia");

        Product product1 = service.CreateNewProduct("Iphone14");
        Product product2 = service.CreateNewProduct("Iphone13");
        Product product3 = service.CreateNewProduct("Iphone12");

        Product iphone14 = service.CreateNewProduct("Iphone14");
        Product iphone13 = service.CreateNewProduct("Iphone13");
        Product iphone12 = service.CreateNewProduct("Iphone12");

        Order order = service.CreateNewOrder();
        Order order2 = service.CreateNewOrder();

        service.AddProductToOrder(order, product1, 140000, 10);
        service.AddProductToOrder(order, product2, 120000, 5);
        service.AddProductToOrder(order, product3, 100000, 7);

        service.DeliverItemsToShop(apple, order);

        service.AddProductToOrder(order2, iphone14, 100000, 10);
        service.AddProductToOrder(order2, iphone13, 90000, 5);
        service.AddProductToOrder(order2, iphone12, 70000, 7);

        service.DeliverItemsToShop(reStore, order2);

        service.AddProductToCart("Iphone14", 1, nikita);
        service.AddProductToCart("Iphone13", 50, nikita);

        Assert.Throws<ServiceException>(() => service.SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible(nikita));
    }

    [Fact]
    public void SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible_NotEnoughProducts()
    {
        User nikita = service.AddNewUser("Nikita", 1000000);

        Shop apple = service.AddNewShop("Apple", "California, USA");
        Shop reStore = service.AddNewShop("reStore", "St.Perersburg, Russia");

        Product product1 = service.CreateNewProduct("Iphone14");
        Product product2 = service.CreateNewProduct("Iphone13");
        Product product3 = service.CreateNewProduct("Iphone12");

        Product iphone14 = service.CreateNewProduct("Iphone14");
        Product iphone13 = service.CreateNewProduct("Iphone13");
        Product iphone12 = service.CreateNewProduct("Iphone12");

        Order order = service.CreateNewOrder();
        Order order2 = service.CreateNewOrder();

        service.AddProductToOrder(order, product1, 140000, 10);
        service.AddProductToOrder(order, product2, 120000, 5);

        service.DeliverItemsToShop(apple, order);

        service.AddProductToOrder(order2, iphone14, 100000, 10);
        service.AddProductToOrder(order2, iphone13, 90000, 5);

        service.DeliverItemsToShop(reStore, order2);

        service.AddProductToCart("Iphone14", 1, nikita);
        service.AddProductToCart("Iphone12", 1, nikita);

        Assert.Throws<ServiceException>(() => service.SearchForShopWhereOrderCanBeBoughtAsCheaplyAsPossible(nikita));
    }

    [Fact]
    public void BuyProductsInOrder()
    {
        decimal oldMoney = 5000;
        decimal firstPrice = 1400;
        decimal secondPrice = 1000;
        int firstAmount = 2;
        int secondAmount = 5;
        int countOfItems = 1;

        User newBuyer = service.AddNewUser("russianZAK", oldMoney);

        Product watch1 = service.CreateNewProduct("watch1");
        Product watch2 = service.CreateNewProduct("watch2");
        Product watch3 = service.CreateNewProduct("watch3");
        Order order = service.CreateNewOrder();
        Shop watchShop = service.AddNewShop("Wasthesss", "St.Petersburg, Russia");

        service.AddProductToOrder(order, watch1, firstPrice, firstAmount);
        service.AddProductToOrder(order, watch2, secondPrice, secondAmount);
        service.AddProductToOrder(order, watch3, 99000, 3);

        service.DeliverItemsToShop(watchShop, order);

        service.AddProductToCart("watch1", countOfItems, newBuyer);
        service.AddProductToCart("watch2", countOfItems, newBuyer);

        service.BuyProducts(newBuyer, watchShop);

        Assert.Equal(newBuyer.Money, oldMoney - ((firstPrice * countOfItems) + (secondPrice * countOfItems)));
        Assert.Equal(firstAmount - countOfItems, service.GetProductInfo(watchShop, watch1));
        Assert.Equal(secondAmount - countOfItems, service.GetProductInfo(watchShop, watch2));
    }

    [Fact]
    public void BuyProductsInOrder_BuyerDoesntHaveEnoughMoney()
    {
        User newBuyer = service.AddNewUser("russianZAK", 1000);

        Product watch1 = service.CreateNewProduct("watch1");
        Product watch2 = service.CreateNewProduct("watch2");
        Product watch3 = service.CreateNewProduct("watch3");
        Order order = service.CreateNewOrder();
        Shop watchShop = service.AddNewShop("Wasthesss", "St.Petersburg, Russia");

        service.AddProductToOrder(order, watch1, 500, 8);

        service.DeliverItemsToShop(watchShop, order);

        service.AddProductToCart("watch1", 7, newBuyer);

        Assert.Throws<UserException>(() => service.BuyProducts(newBuyer, watchShop));
    }

    [Fact]
    public void BuyProductsInOrder_NotEnoughAmountOfProductsInShop()
    {
        User newBuyer = service.AddNewUser("russianZAK", 100000);

        Product watch1 = service.CreateNewProduct("watch1");
        Product watch2 = service.CreateNewProduct("watch2");
        Product watch3 = service.CreateNewProduct("watch3");
        Shop watchShop = service.AddNewShop("Wasthesss", "St.Petersburg, Russia");
        Order order = service.CreateNewOrder();

        service.AddProductToOrder(order, watch1, 500, 8);

        service.DeliverItemsToShop(watchShop, order);

        service.AddProductToCart("watch1", 9, newBuyer);

        Assert.Throws<ShopException>(() => service.BuyProducts(newBuyer, watchShop));
    }

    [Fact]
    public void BuyProductsInOrder_NotEnoughProductsInShop()
    {
        User newBuyer = service.AddNewUser("russianZAK", 100000);

        Product watch1 = service.CreateNewProduct("watch1");
        Product watch2 = service.CreateNewProduct("watch2");
        Product watch3 = service.CreateNewProduct("watch3");
        Order order = service.CreateNewOrder();
        Shop watchShop = service.AddNewShop("Wasthesss", "St.Petersburg, Russia");

        service.AddProductToOrder(order, watch1, 500, 8);

        service.DeliverItemsToShop(watchShop, order);

        service.AddProductToCart("watch2", 9, newBuyer);

        Assert.Throws<ShopException>(() => service.BuyProducts(newBuyer, watchShop));
    }
}
