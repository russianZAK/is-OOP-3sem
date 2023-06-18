using Banks.Entities;
using Banks.Models;
using Banks.Service;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void TopUpBankAccount()
    {
        var timeProver = new TimeProvider();
        var centralBank = new CentralBank(timeProver);
        Bank sber = centralBank.AddNewBank("Sber", 5, 3, 4, 5, 400, 10000, 50000);

        Client newClient = new Client.Builder().SetFirstName("Fisenko").SetSurname("Nikita").SetAddress("Teatralnaya 354").SetPassport("1017 3434345").Build();
        sber.AddNewClient(newClient);
        sber.AddNewDebitAccountToClient(newClient);
        IBankAccount debitAccount = newClient.BankAccounts.First();
        decimal newBalance = 4000;
        sber.MoneyTopUpTransaction(debitAccount, newBalance);
        Assert.Equal(debitAccount.Balance, newBalance);
    }

    [Fact]
    public void RollbackOperation()
    {
        var timeProver = new TimeProvider();
        var centralBank = new CentralBank(timeProver);
        Bank sber = centralBank.AddNewBank("Sber", 5, 3, 4, 5, 400, 10000, 50000);

        Client newClient = new Client.Builder().SetFirstName("Fisenko").SetSurname("Nikita").SetAddress("Teatralnaya 354").SetPassport("1017 3434345").Build();
        sber.AddNewClient(newClient);
        sber.AddNewDebitAccountToClient(newClient);
        IBankAccount debitAccount = newClient.BankAccounts.First();
        decimal newBalance = 4000;
        sber.MoneyTopUpTransaction(debitAccount, newBalance);
        decimal startBalance = 0;

        ITransaction transaction = sber.Transactions.First();
        transaction.Rollback();

        Assert.Equal(debitAccount.Balance, startBalance);
    }

    [Fact]
    public void DateChange()
    {
        var timeProver = new TimeProvider(new DateTime(2022, 11, 26));
        var centralBank = new CentralBank(timeProver);
        decimal percentForDebitAccounts = 5;
        Bank sber = centralBank.AddNewBank("Sber", percentForDebitAccounts, 3, 4, 5, 400, 10000, 50000);

        Client newClient = new Client.Builder()
            .SetFirstName("Fisenko")
            .SetSurname("Nikita")
            .SetAddress("Teatralnaya 354")
            .SetPassport("1017 3434345")
            .Build();

        sber.AddNewClient(newClient);
        sber.AddNewDebitAccountToClient(newClient);
        IBankAccount debitAccount = newClient.BankAccounts.First();
        decimal newBalance = 4000;
        sber.MoneyTopUpTransaction(debitAccount, newBalance);

        var timeProverForChangingDate = new TimeProvider();
        timeProverForChangingDate.ChangeDate(timeProver.Date.AddMonths(1));
        centralBank.ChangeDate(timeProverForChangingDate.Date);

        decimal dayPercent = percentForDebitAccounts / 365;
        decimal newAmountOfMoney = 0;

        for (int i = 0; i < 5; i++)
        {
            newAmountOfMoney += newBalance * dayPercent;
        }

        Assert.Equal(debitAccount.Balance, newAmountOfMoney + newBalance);
    }
}