using Banks.Exceptions;

namespace Banks.Entities;

public class BankAccountId
{
    public BankAccountId(int bankId, Guid accountId)
    {
        if (bankId < 0) throw BankAccountIdException.InvalidId(BankId);

        BankId = bankId;
        AccountId = accountId;
    }

    public int BankId { get; }
    public Guid AccountId { get; }
}
