namespace Banks.Exceptions;

public class BankException : Exception
{
    private BankException(string? message)
   : base(message)
        { }

    public static BankException InvalidName()
    {
        return new BankException("name is invalid");
    }

    public static BankException InvalidId()
    {
        return new BankException("id is invalid");
    }

    public static BankException InvalidPercentForDebitAccounts(decimal percentForDebitAccounts)
    {
        return new BankException($"{percentForDebitAccounts} is invalid");
    }

    public static BankException InvalidPercentLessFiftyThousand(decimal percentLessFiftyThousand)
    {
        return new BankException($"{percentLessFiftyThousand} is invalid");
    }

    public static BankException InvalidPercentLessOneHundredThousand(decimal percentLessOneHundredThousand)
    {
        return new BankException($"{percentLessOneHundredThousand} is invalid");
    }

    public static BankException InvalidPercentMoreOneHundredThousand(decimal percentMoreOneHundredThousand)
    {
        return new BankException($"{percentMoreOneHundredThousand} is invalid");
    }

    public static BankException InvalidCommissionForCreditAccount(decimal commissionForCreditAccount)
    {
        return new BankException($"{commissionForCreditAccount} is invalid");
    }

    public static BankException InvalidRestrictionForNotVerifiedCustomers(decimal restrictionForNotVerifiedCustomers)
    {
        return new BankException($"{restrictionForNotVerifiedCustomers} is invalid");
    }

    public static BankException InvalidCreditLimit(decimal creditLimit)
    {
        return new BankException($"{creditLimit} is invalid");
    }

    public static BankException InvalidClient()
    {
        return new BankException("client is invalid");
    }

    public static BankException InvalidBankAccount()
    {
        return new BankException("Bank account is invalid");
    }

    public static BankException InvalidTransaction()
    {
        return new BankException("Transaction is invalid");
    }

    public static BankException ClientDoesntExistInSystem()
    {
        return new BankException("client doesn't exist in system");
    }

    public static BankException BankAccountDoesntExistInSystem()
    {
        return new BankException("Bank account doesn't exist in system");
    }

    public static BankException TransactionDoesntExistInSystem()
    {
        return new BankException("Transaction doesn't exist in system");
    }

    public static BankException InvalidAmountOfMoney(decimal money)
    {
        return new BankException($"{money} is invalid");
    }

    public static BankException InvalidOperation()
    {
        return new BankException("operation is invalid");
    }
}
