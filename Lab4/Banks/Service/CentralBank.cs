using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;

namespace Banks.Service;

public class CentralBank : ISubject
{
    private List<Bank> _banks;
    private List<MoneyTransferTransaction> _transactions;
    private List<IWatcher> _watchers;
    private int _id;
    public CentralBank(TimeProvider timeProvider)
    {
        _id = 0;
        _banks = new List<Bank>();
        _transactions = new List<MoneyTransferTransaction>();
        TimeProvider = timeProvider;
        _watchers = new List<IWatcher>();
    }

    public IReadOnlyCollection<Bank> Banks => _banks;
    public IReadOnlyCollection<IWatcher> Watchers => _watchers;
    public IReadOnlyCollection<MoneyTransferTransaction> Transactions => _transactions;
    public TimeProvider TimeProvider { get; }

    public Bank AddNewBank(string name, decimal percentForDebitAccounts, decimal percentLessFiftyThousand, decimal percentLessOneHundredThousand, decimal percentMoreOneHundredThousand, decimal commissionForCreditAccount, decimal restrictionForNotVerifiedCustomers, decimal creditLimit)
    {
        if (name == null) throw BankException.InvalidName();
        if (percentForDebitAccounts < 0) throw BankException.InvalidPercentForDebitAccounts(percentForDebitAccounts);
        if (percentLessFiftyThousand < 0) throw BankException.InvalidPercentLessFiftyThousand(percentLessFiftyThousand);
        if (percentLessOneHundredThousand < 0) throw BankException.InvalidPercentLessOneHundredThousand(percentLessOneHundredThousand);
        if (percentMoreOneHundredThousand < 0) throw BankException.InvalidPercentMoreOneHundredThousand(percentMoreOneHundredThousand);
        if (commissionForCreditAccount < 0) throw BankException.InvalidCommissionForCreditAccount(commissionForCreditAccount);
        if (restrictionForNotVerifiedCustomers < 0) throw BankException.InvalidRestrictionForNotVerifiedCustomers(restrictionForNotVerifiedCustomers);
        if (creditLimit < 0) throw BankException.InvalidCreditLimit(creditLimit);

        var newBank = new Bank(name, _id, percentForDebitAccounts, percentLessFiftyThousand, percentLessOneHundredThousand, percentMoreOneHundredThousand, commissionForCreditAccount, restrictionForNotVerifiedCustomers, creditLimit);
        _banks.Add(newBank);
        _watchers.Add(newBank);
        _id++;
        return newBank;
    }

    public MoneyTransferTransaction MoneyTransferTransaction(IBankAccount bankAccountFrom, IBankAccount bankAccountTo, decimal money)
    {
        if (bankAccountFrom == null) throw BankException.InvalidBankAccount();
        if (bankAccountTo == null) throw BankException.InvalidBankAccount();
        if (money < 0) throw BankException.InvalidAmountOfMoney(money);

        if (!bankAccountFrom.Owner.IsVerified && money > bankAccountFrom.Bank.RestrictionForNotVerifiedCustomers) throw BankException.InvalidOperation();

        var newMoneyTransferTransaction = new MoneyTransferTransaction(bankAccountFrom, bankAccountTo, money);
        newMoneyTransferTransaction.Execute();
        _transactions.Add(newMoneyTransferTransaction);

        bankAccountFrom.Bank.AddMoneyTransferTransaction(newMoneyTransferTransaction);
        bankAccountTo.Bank.AddMoneyTransferTransaction(newMoneyTransferTransaction);

        return newMoneyTransferTransaction;
    }

    public MoneyTransferTransaction Rollback(MoneyTransferTransaction transaction)
    {
        if (transaction == null) throw BankException.InvalidTransaction();
        if (!_transactions.Contains(transaction)) throw BankException.TransactionDoesntExistInSystem();

        transaction.Rollback();

        return transaction;
    }

    public void Attach(IWatcher watcher)
    {
        if (watcher == null) throw CentralBankException.InvalidWatcher();
        _watchers.Add(watcher);
    }

    public void Detach(IWatcher watcher)
    {
        if (watcher == null) throw CentralBankException.InvalidWatcher();
        _watchers.Remove(watcher);
    }

    public void ChangeDate(DateTime time)
    {
        while (DateOnly.FromDateTime(TimeProvider.Date) != DateOnly.FromDateTime(time.Date))
        {
            _watchers.ForEach(bank =>
            {
                bank.UpdateDate(TimeProvider.Date);
            });

            TimeProvider.ChangeDate(TimeProvider.Date.AddDays(1));
        }
    }
}
