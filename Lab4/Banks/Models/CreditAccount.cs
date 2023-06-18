using Banks.Entities;
using Banks.Exceptions;

namespace Banks.Models;

public class CreditAccount : IBankAccount
{
    public CreditAccount(BankAccountId accountId, Client owner, decimal creditLimit, decimal commission, Bank bank)
    {
        if (creditLimit < 0) throw BankAccountException.InvalidCreditLimit(creditLimit);
        if (owner == null) throw BankAccountException.InvalidClient();
        if (accountId == null) throw BankAccountException.InvalidBankAccountId();
        if (commission < 0) throw BankAccountException.InvalidCommissionForCreditAccount(commission);
        if (bank == null) throw BankAccountException.InvalidBank();

        Balance = creditLimit;
        AccountId = accountId;
        Owner = owner;
        Commission = commission;
        CreditLimit = creditLimit;
        Bank = bank;
    }

    public decimal Commission { get; private set; }
    public decimal Balance { get; private set; }
    public decimal Debt { get; private set; }
    public decimal CreditLimit { get; private set; }
    public BankAccountId AccountId { get; }
    public Client Owner { get; }
    public Bank Bank { get; }

    public void DayChange(bool islastDayOfMonth)
    {
        if (Balance < CreditLimit)
        {
            Debt += Commission;
        }

        if (islastDayOfMonth)
        {
            Balance -= Debt;
            Debt = 0;
        }
    }

    public decimal GetLimit()
    {
        return CreditLimit;
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
