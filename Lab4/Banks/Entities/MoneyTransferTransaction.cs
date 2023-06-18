using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities;

public class MoneyTransferTransaction : ITransaction
{
    public MoneyTransferTransaction(IBankAccount bankAccountFrom, IBankAccount bankAccountTo, decimal money)
    {
        if (bankAccountFrom == null) throw TransactionException.InvalidBankAccount();
        if (bankAccountTo == null) throw TransactionException.InvalidBankAccount();
        if (money < 0) throw TransactionException.InvalidMoney(money);

        AccountFrom = bankAccountFrom;
        AccountTo = bankAccountTo;
        Money = money;
        IsMoneyTransferred = false;
    }

    public IBankAccount AccountFrom { get; }
    public IBankAccount AccountTo { get; }
    public decimal Money { get; }
    public bool IsMoneyTransferred { get; private set; }

    public void Execute()
    {
        if (IsMoneyTransferred)
        {
            throw TransactionException.InvalidTransaction();
        }

        if (!AccountFrom.IsWithdrawAllowed(Money)) throw TransactionException.InvalidTransaction();
        AccountFrom.Withdraw(Money);
        AccountTo.TopUp(Money);

        IsMoneyTransferred = true;
    }

    public void Rollback()
    {
        if (!IsMoneyTransferred) throw TransactionException.InvalidTransaction();
        AccountTo.Withdraw(Money);
        AccountFrom.TopUp(Money);
    }
}
