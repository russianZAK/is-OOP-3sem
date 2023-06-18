using Banks.Entities;
using Banks.Exceptions;

namespace Banks.Models;

public class DebitAccount : IBankAccount
{
    public DebitAccount(BankAccountId accountId, Client owner, decimal percent, Bank bank)
    {
        if (percent < 0) throw BankAccountException.InvalidPercentForDebitAccounts(percent);
        if (owner == null) throw BankAccountException.InvalidClient();
        if (accountId == null) throw BankAccountException.InvalidBankAccountId();
        if (bank == null) throw BankAccountException.InvalidBank();

        Balance = 0;
        Accruals = 0;
        AccountId = accountId;
        Owner = owner;
        Percent = percent;
        Bank = bank;
    }

    public decimal Percent { get; private set; }
    public decimal Accruals { get; private set; }
    public decimal Balance { get; private set; }
    public Bank Bank { get; }
    public BankAccountId AccountId { get; }

    public Client Owner { get; }

    public void DayChange(bool islastDayOfMonth)
    {
        decimal dayPercent = Percent / 365;
        Accruals += Balance * dayPercent;

        if (islastDayOfMonth)
        {
            Balance += Accruals;
            Accruals = 0;
        }
    }

    public decimal GetLimit()
    {
        return Balance;
    }

    public bool IsWithdrawAllowed(decimal amount)
    {
        if (amount < 0) throw BankAccountException.InvalidAmountOfMoney(amount);
        if (Balance - amount >= 0) return true;
        else return false;
    }

    public void TopUp(decimal amount)
    {
        if (amount < 0) throw BankAccountException.InvalidAmountOfMoney(amount);
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount < 0) throw BankAccountException.InvalidAmountOfMoney(amount);
        Balance -= amount;
    }
}
