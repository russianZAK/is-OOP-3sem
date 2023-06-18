namespace Banks.Exceptions;

public class BankAccountException : Exception
{
    private BankAccountException(string? message)
   : base(message)
    { }

    public static BankAccountException InvalidPercentForDebitAccounts(decimal percentForDebitAccounts)
    {
        return new BankAccountException($"{percentForDebitAccounts} is invalid");
    }

    public static BankAccountException InvalidPercentLessFiftyThousand(decimal percentLessFiftyThousand)
    {
        return new BankAccountException($"{percentLessFiftyThousand} is invalid");
    }

    public static BankAccountException InvalidPercentLessOneHundredThousand(decimal percentLessOneHundredThousand)
    {
        return new BankAccountException($"{percentLessOneHundredThousand} is invalid");
    }

    public static BankAccountException InvalidPercentMoreOneHundredThousand(decimal percentMoreOneHundredThousand)
    {
        return new BankAccountException($"{percentMoreOneHundredThousand} is invalid");
    }

    public static BankAccountException InvalidCommissionForCreditAccount(decimal commissionForCreditAccount)
    {
        return new BankAccountException($"{commissionForCreditAccount} is invalid");
    }

    public static BankAccountException InvalidRestrictionForNotVerifiedCustomers(decimal restrictionForNotVerifiedCustomers)
    {
        return new BankAccountException($"{restrictionForNotVerifiedCustomers} is invalid");
    }

    public static BankAccountException InvalidCreditLimit(decimal creditLimit)
    {
        return new BankAccountException($"{creditLimit} is invalid");
    }

    public static BankAccountException InvalidAmountOfMoney(decimal money)
    {
        return new BankAccountException($"{money} is invalid");
    }

    public static BankAccountException InvalidClient()
    {
        return new BankAccountException("client is invalid");
    }

    public static BankAccountException InvalidBankAccountId()
    {
        return new BankAccountException("bank account id is invalid");
    }

    public static BankAccountException InvalidBank()
    {
        return new BankAccountException("bank id is invalid");
    }

    public static BankAccountException InvalidPercent(decimal percent)
    {
        return new BankAccountException($"{percent} is invalid");
    }

    public static BankAccountException InvalidBalance(decimal balance)
    {
        return new BankAccountException($"{balance} is invalid");
    }
}
