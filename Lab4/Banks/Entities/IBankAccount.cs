using Banks.Entities;

namespace Banks.Models;

public interface IBankAccount
{
     decimal Balance { get; }
     Bank Bank { get; }
     BankAccountId AccountId { get; }
     Client Owner { get; }
     void DayChange(bool islastDayOfMonth);
     void TopUp(decimal amount);
     void Withdraw(decimal amount);
     bool IsWithdrawAllowed(decimal amount);
     decimal GetLimit();
}
