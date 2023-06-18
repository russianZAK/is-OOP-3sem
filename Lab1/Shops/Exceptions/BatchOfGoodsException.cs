namespace Shops.Exceptions;

public class BatchOfGoodsException : Exception
{
    private BatchOfGoodsException(string? message)
        : base(message)
    {
    }

    public static BatchOfGoodsException InvalidPricePerItem(decimal price)
    {
        throw new BatchOfGoodsException($"{price} is invalid");
    }

    public static BatchOfGoodsException InvalidAmount(int amount)
    {
        throw new BatchOfGoodsException($"{amount} is invalid");
    }

    public static BatchOfGoodsException InvalidBatchOfGoods()
    {
        throw new BatchOfGoodsException("batch of goods is invalid");
    }
}
