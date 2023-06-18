using Banks.Entities;
using Banks.Exceptions;
using Banks.Notifications;

namespace Banks.Models;

public class Bank : IWatcher
{
    private List<Client> _clients;
    private List<IBankAccount> _bankAccounts;
    private List<ITransaction> _transactions;
    private ChangePercentsForDebitAccountAggregator _changePercentsForDebitAccountAggregator;
    private ChangeCommissionForCreditAccountAggregator _changeCommissionForCreditAccountAggregator;
    private ChangeCreditLimitAggregator _changeCreditLimitAggregator;
    private ChangeRestrictionForNotVerifiedCustomersAggregator _changeRestrictionForNotVerifiedCustomersAggregator;
    private ChangePercentForDepositAccountAggregator _changePercentForDepositAccountAggregator;

    public Bank(
        string name,
        int id,
        decimal
        percentForDebitAccounts,
        decimal percentLessFiftyThousand,
        decimal percentLessOneHundredThousand,
        decimal percentMoreOneHundredThousand,
        decimal commissionForCreditAccount,
        decimal restrictionForNotVerifiedCustomers,
        decimal creditLimit)
    {
        _clients = new List<Client>();
        _transactions = new List<ITransaction>();
        _bankAccounts = new List<IBankAccount>();
        _changePercentsForDebitAccountAggregator = new ChangePercentsForDebitAccountAggregator();
        _changeCommissionForCreditAccountAggregator = new ChangeCommissionForCreditAccountAggregator();
        _changeCreditLimitAggregator = new ChangeCreditLimitAggregator();
        _changeRestrictionForNotVerifiedCustomersAggregator = new ChangeRestrictionForNotVerifiedCustomersAggregator();
        _changePercentForDepositAccountAggregator = new ChangePercentForDepositAccountAggregator();

        if (name == null) throw BankException.InvalidName();
        if (id < 0) throw BankException.InvalidId();
        if (percentForDebitAccounts < 0) throw BankException.InvalidPercentForDebitAccounts(percentForDebitAccounts);
        if (percentLessFiftyThousand < 0) throw BankException.InvalidPercentLessFiftyThousand(percentLessFiftyThousand);
        if (percentLessOneHundredThousand < 0) throw BankException.InvalidPercentLessOneHundredThousand(percentLessOneHundredThousand);
        if (percentMoreOneHundredThousand < 0) throw BankException.InvalidPercentMoreOneHundredThousand(percentMoreOneHundredThousand);
        if (commissionForCreditAccount < 0) throw BankException.InvalidCommissionForCreditAccount(commissionForCreditAccount);
        if (restrictionForNotVerifiedCustomers < 0) throw BankException.InvalidRestrictionForNotVerifiedCustomers(restrictionForNotVerifiedCustomers);
        if (creditLimit < 0) throw BankException.InvalidCreditLimit(creditLimit);

        Name = name;
        Id = id;
        PercentForDebitAccounts = percentForDebitAccounts;
        PercentLessFiftyThousand = percentLessFiftyThousand;
        PercentLessOneHundredThousand = percentLessOneHundredThousand;
        PercentMoreOneHundredThousand = percentMoreOneHundredThousand;
        CommissionForCreditAccount = commissionForCreditAccount;
        RestrictionForNotVerifiedCustomers = restrictionForNotVerifiedCustomers;
        CreditLimit = creditLimit;
    }

    public string Name { get; }
    public int Id { get; }
    public decimal PercentForDebitAccounts { get; private set; }
    public decimal PercentLessFiftyThousand { get; private set; }
    public decimal PercentLessOneHundredThousand { get; private set; }
    public decimal PercentMoreOneHundredThousand { get; private set; }
    public decimal CommissionForCreditAccount { get; private set; }
    public decimal RestrictionForNotVerifiedCustomers { get; private set; }
    public decimal CreditLimit { get; private set; }

    public IReadOnlyCollection<Client> Clients => _clients;
    public IReadOnlyCollection<IBankAccount> BankAccounts => _bankAccounts;
    public IReadOnlyCollection<ITransaction> Transactions => _transactions;

    public void AddNewClient(Client client)
    {
        if (client == null) throw BankException.InvalidClient();

        if (!client.IsVerified) _changeRestrictionForNotVerifiedCustomersAggregator.Subscribe(client);
        client.AddBank(this);
        _clients.Add(client);
    }

    public DebitAccount AddNewDebitAccountToClient(Client client)
    {
        if (client == null) throw BankException.InvalidClient();
        if (!_clients.Contains(client)) throw BankException.ClientDoesntExistInSystem();

        var newDebitAccount = new DebitAccount(new BankAccountId(Id, Guid.NewGuid()), client, PercentForDebitAccounts, this);
        _changePercentsForDebitAccountAggregator.Subscribe(client);
        client.AddBankAccount(newDebitAccount);
        _bankAccounts.Add(newDebitAccount);
        return newDebitAccount;
    }

    public CreditAccount AddNewCreditAccountToClient(Client client)
    {
        if (client == null) throw BankException.InvalidClient();
        if (!_clients.Contains(client)) throw BankException.ClientDoesntExistInSystem();

        var newCreditAccount = new CreditAccount(new BankAccountId(Id, Guid.NewGuid()), client, CreditLimit, CommissionForCreditAccount, this);
        _changeCommissionForCreditAccountAggregator.Subscribe(client);
        _changeCreditLimitAggregator.Subscribe(client);
        client.AddBankAccount(newCreditAccount);
        _bankAccounts.Add(newCreditAccount);
        return newCreditAccount;
    }

    public DepositAccount AddNewDepositAccountToClient(Client client, decimal money, DateTime time)
    {
        if (client == null) throw BankException.InvalidClient();
        if (!_clients.Contains(client)) throw BankException.ClientDoesntExistInSystem();
        if (money < 0) throw BankException.InvalidAmountOfMoney(money);

        decimal percent = 0;

        if (money < 50000) percent = PercentLessFiftyThousand;
        if (money >= 50000 && money < 100000) percent = PercentLessOneHundredThousand;
        if (money >= 100000) percent = PercentMoreOneHundredThousand;

        var newDepositAccount = new DepositAccount(new BankAccountId(Id, Guid.NewGuid()), client, percent, time, money, this);
        _changePercentForDepositAccountAggregator.Subscribe(client);
        client.AddBankAccount(newDepositAccount);
        _bankAccounts.Add(newDepositAccount);
        return newDepositAccount;
    }

    public MoneyTopUpTransaction MoneyTopUpTransaction(IBankAccount bankAccount, decimal money)
    {
        if (money < 0) throw BankException.InvalidAmountOfMoney(money);
        if (bankAccount == null) throw BankException.InvalidBankAccount();
        if (!_bankAccounts.Contains(bankAccount)) throw BankException.BankAccountDoesntExistInSystem();

        var newMoneyTopUpTransaction = new MoneyTopUpTransaction(bankAccount, money);
        newMoneyTopUpTransaction.Execute();
        _transactions.Add(newMoneyTopUpTransaction);
        return newMoneyTopUpTransaction;
    }

    public MoneyWithdrawTransaction MoneyWithdrawTransaction(IBankAccount bankAccount, decimal money)
    {
        if (money < 0) throw BankException.InvalidAmountOfMoney(money);
        if (bankAccount == null) throw BankException.InvalidBankAccount();
        if (!_bankAccounts.Contains(bankAccount)) throw BankException.BankAccountDoesntExistInSystem();

        if (!bankAccount.Owner.IsVerified && money > RestrictionForNotVerifiedCustomers) throw BankException.InvalidOperation();

        var newMoneyWithdrawTransaction = new MoneyWithdrawTransaction(bankAccount, money);
        newMoneyWithdrawTransaction.Execute();
        _transactions.Add(newMoneyWithdrawTransaction);
        return newMoneyWithdrawTransaction;
    }

    public void AddMoneyTransferTransaction(ITransaction transaction)
    {
        if (transaction == null) throw BankException.InvalidTransaction();
        _transactions.Add(transaction);
    }

    public ITransaction Rollback(ITransaction transaction)
    {
        if (transaction == null) throw BankException.InvalidTransaction();
        if (!_transactions.Contains(transaction)) throw BankException.TransactionDoesntExistInSystem();

        transaction.Rollback();

        return transaction;
    }

    public MoneyTransferTransaction MoneyTransferTransaction(IBankAccount bankAccountFrom, IBankAccount bankAccountTo, decimal money)
    {
        if (bankAccountFrom == null) throw BankException.InvalidBankAccount();
        if (bankAccountTo == null) throw BankException.InvalidBankAccount();
        if (!_bankAccounts.Contains(bankAccountFrom)) throw BankException.BankAccountDoesntExistInSystem();
        if (!_bankAccounts.Contains(bankAccountTo)) throw BankException.BankAccountDoesntExistInSystem();
        if (money < 0) throw BankException.InvalidAmountOfMoney(money);

        if (!bankAccountFrom.Owner.IsVerified && money > RestrictionForNotVerifiedCustomers) throw BankException.InvalidOperation();

        var newMoneyTransferTransaction = new MoneyTransferTransaction(bankAccountFrom, bankAccountTo, money);
        newMoneyTransferTransaction.Execute();
        _transactions.Add(newMoneyTransferTransaction);
        return newMoneyTransferTransaction;
    }

    public void ChangePercentForDebitAccounts(decimal newPercent)
    {
        if (newPercent < 0) throw BankException.InvalidPercentForDebitAccounts(newPercent);

        PercentForDebitAccounts = newPercent;
        var newNotification = new ChangePercentsForDebitAccountNotification($"{newPercent} - new percent for new debit accounts");
        _changePercentsForDebitAccountAggregator.Notify(newNotification);
    }

    public void ChangeCommissionForCreditAccount(decimal newCommission)
    {
        if (newCommission < 0) throw BankException.InvalidCommissionForCreditAccount(newCommission);

        CommissionForCreditAccount = newCommission;
        var newNotification = new ChangeCommissionForCreditAccountNotification($"{newCommission} - new commission for credit accounts");
        _changeCommissionForCreditAccountAggregator.Notify(newNotification);
    }

    public void ChangeCreditLimit(decimal newCreditLimit)
    {
        if (newCreditLimit < 0) throw BankException.InvalidCreditLimit(newCreditLimit);

        CreditLimit = newCreditLimit;
        var newNotification = new ChangeCreditLimitNotification($"{newCreditLimit} - new creditlimit for credit accounts");
        _changeCreditLimitAggregator.Notify(newNotification);
    }

    public void ChangeRestrictionForNotVerifiedCustomersAggregator(decimal newRestriction)
    {
        if (newRestriction < 0) throw BankException.InvalidRestrictionForNotVerifiedCustomers(newRestriction);

        RestrictionForNotVerifiedCustomers = newRestriction;
        var newNotification = new ChangeRestrictionForNotVerifiedCustomersNotification($"{newRestriction} - new restriction for not verified clients");
        _changeRestrictionForNotVerifiedCustomersAggregator.Notify(newNotification);
    }

    public void ChangePercentLessFiftyThousand(decimal newPercent)
    {
        if (newPercent < 0) throw BankException.InvalidPercentLessFiftyThousand(newPercent);

        PercentLessFiftyThousand = newPercent;
        var newNotification = new ChangePercentForDepositAccountNotification($"PercentLessFiftyThousand {newPercent} !");
        _changePercentForDepositAccountAggregator.Notify(newNotification);
    }

    public void ChangePercentLessOneHundredThousand(decimal newPercent)
    {
        if (newPercent < 0) throw BankException.InvalidPercentLessOneHundredThousand(newPercent);

        PercentLessOneHundredThousand = newPercent;
        var newNotification = new ChangePercentForDepositAccountNotification($"PercentLessOneHundredThousand {newPercent} !");
        _changePercentForDepositAccountAggregator.Notify(newNotification);
    }

    public void ChangePercentMoreOneHundredThousand(decimal newPercent)
    {
        if (newPercent < 0) throw BankException.InvalidPercentMoreOneHundredThousand(newPercent);

        PercentMoreOneHundredThousand = newPercent;
        var newNotification = new ChangePercentForDepositAccountNotification($"PercentMoreOneHundredThousand {newPercent} !");
        _changePercentForDepositAccountAggregator.Notify(newNotification);
    }

    public void UpdateDate(DateTime time)
    {
        int month = time.Month;
        int year = time.Year;
        int allDaysInMonth = DateTime.DaysInMonth(year, month);
        bool isLastDayOfMonth = false;
        var lastDayOfMonth = new DateTime(year, month, allDaysInMonth);

        if (DateOnly.FromDateTime(lastDayOfMonth) == DateOnly.FromDateTime(time))
        {
            isLastDayOfMonth = true;
        }
        else
        {
            isLastDayOfMonth = false;
        }

        _bankAccounts.ForEach(bankAccount =>
        {
            bankAccount.DayChange(isLastDayOfMonth);
        });
    }
}
