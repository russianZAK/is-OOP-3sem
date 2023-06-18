using Banks.Entities;
using Banks.Exceptions;

namespace Banks.Models;

public class DepositAccount : IBankAccount
{
    public DepositAccount(BankAccountId accountId, Client owner, decimal percent, DateTime date, decimal balance, Bank bank)
    {
        if (percent < 0) throw BankAccountException.InvalidPercent(percent);
        if (owner == null) throw BankAccountException.InvalidClient();
        if (accountId == null) throw BankAccountException.InvalidBankAccountId();
        if (balance < 0) throw BankAccountException.InvalidBalance(balance);
        if (bank == null) throw BankAccountException.InvalidBank();

        Balance = balance;
        AccountId = accountId;
        Owner = owner;
        DateOfDeadline = date;
        Percent = percent;
        IsDeadlineEnd = false;
        Accruals = 0;
        Bank = bank;
        Date = DateTime.Now;
    }

    public DateTime DateOfDeadline { get; }
    public DateTime Date { get; }
    public decimal Percent { get; private set; }
    public decimal Accruals { get; private set; }
    public decimal Balance { get; private set; }
    public Bank Bank { get; }
    public BankAccountId AccountId { get; }

    public Client Owner { get; }

    public bool IsDeadlineEnd { get; private set; }

    public void DayChange(bool islastDayOfMonth)
    {
        Date.AddDays(1);
        if (DateOnly.FromDateTime(DateOfDeadline) == DateOnly.FromDateTime(Date))
        {
            IsDeadlineEnd = true;
        }

        decimal dayPercent = Percent / 365;
        Accruals += Balance * dayPercent;

        if (islastDayOfMonth && !IsDeadlineEnd)
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
        if (!IsDeadlineEnd) return false;

        if (Balance - amount >= 0) return true;
        else return false;
    }

    public void TopUp(decimal amount)
    {
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        Balance -= amount;
    }
}
