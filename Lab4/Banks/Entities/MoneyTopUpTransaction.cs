using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities;

public class MoneyTopUpTransaction : ITransaction
{
    public MoneyTopUpTransaction(IBankAccount account, decimal money)
    {
        if (account == null) throw TransactionException.InvalidBankAccount();
        if (money < 0) throw TransactionException.InvalidMoney(money);
        AccountFrom = account;
        AccountTo = account;
        Money = money;
        IsMoneyTransferred = false;
    }

    public decimal Money { get; }
    public bool IsMoneyTransferred { get; private set; }

    public IBankAccount AccountFrom { get; }

    public IBankAccount AccountTo { get; }

    public void Execute()
    {
        if (IsMoneyTransferred) throw TransactionException.InvalidTransaction();
        AccountFrom.TopUp(Money);
        IsMoneyTransferred = true;
    }

    public void Rollback()
    {
        if (!AccountFrom.IsWithdrawAllowed(Money)) throw TransactionException.InvalidTransaction();
        if (!IsMoneyTransferred) throw TransactionException.InvalidTransaction();
        AccountFrom.Withdraw(Money);
    }
}
