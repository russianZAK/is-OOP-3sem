using Banks.Entities;
using Banks.Models;
using Banks.Service;

internal class Program
{
    private static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        var timeProver = new TimeProvider();
        var centralBank = new CentralBank(timeProver);
        centralBank.AddNewBank("Sber", 5, 3, 4, 5, 400, 10000, 50000);
        centralBank.AddNewBank("Tinkoff", 6, 3, 4, 5, 200, 20000, 500000);
        centralBank.AddNewBank("Open", 3, 1, 2, 3, 275, 15000, 200000);
        IBankAccount? bankAccountFrom = null;
        IBankAccount? bankAccountTo = null;
        string? mainMenu = "Main menu";
        int count = 1;
        bool transfer = false;
        while (mainMenu == "Main menu")
        {
            count = 1;
            Console.WriteLine("MENU:");
            Console.WriteLine("\t1)Choose bank");
            Console.WriteLine("\t2)Add new bank");
            Console.WriteLine("\t3)Time");

            int number = Convert.ToInt32(Console.ReadLine());
            string? firstChoice = null;

            switch (number)
            {
                case 1:
                    firstChoice = "Choose bank";
                    break;

                case 2:
                    firstChoice = "Add new bank";
                    break;

                case 3:
                    firstChoice = "Time";
                    break;
            }

            if (firstChoice!.ToLower() == "choose bank")
            {
                Console.Clear();
                Console.WriteLine("MENU:");

                foreach (Bank curtainBank in centralBank.Banks)
                {
                    Console.WriteLine("\t" + count + ")" + " " + curtainBank.Name);
                    count++;
                }

                int indexOfBank = Convert.ToInt32(Console.ReadLine());

                Bank chosenBank = centralBank.Banks.ToList()[indexOfBank - 1];

                Console.Clear();
                Console.WriteLine(chosenBank.Name);
                Console.WriteLine("MENU:");
                Console.WriteLine("\t 1)Register");
                Console.WriteLine("\t 2)Choose account");
                Console.WriteLine("\t 3)Transactions");

                number = Convert.ToInt32(Console.ReadLine());
                string? secondChoice = null;

                switch (number)
                {
                    case 1:
                        secondChoice = "Register";
                        break;

                    case 2:
                        secondChoice = "Choose account";
                        break;

                    case 3:
                        secondChoice = "Transactions";
                        break;
                }

                while (secondChoice != null && secondChoice!.ToLower() == "choose account")
                {
                    Console.Clear();
                    count = 1;
                    chosenBank.Clients.ToList().ForEach(client =>
                    {
                        Console.WriteLine("\t" + count + ")" + " " + client.Name + " " + client.Surname);
                        count++;
                    });

                    int indexOfAccount = Convert.ToInt32(Console.ReadLine());

                    Console.Clear();
                    Client client = chosenBank.Clients.ToList()[indexOfAccount - 1];
                    Console.WriteLine(client.Name + " " + client.Surname);
                    Console.WriteLine("MENU:");
                    Console.WriteLine("\t 1)Open new bank account");
                    Console.WriteLine("\t 2)Bank accounts");
                    Console.WriteLine("\t 3)Back");
                    number = Convert.ToInt32(Console.ReadLine());

                    string? thirdChoice = null;

                    switch (number)
                    {
                        case 1:
                            thirdChoice = "Open new bank account";
                            break;

                        case 2:
                            thirdChoice = "Bank accounts";
                            break;

                        case 3:
                            thirdChoice = "Back";
                            break;
                    }

                    count = 1;

                    if (thirdChoice!.ToLower() == "open new bank account")
                    {
                        Console.Clear();
                        Console.WriteLine("\t 1) Debit account");
                        Console.WriteLine("\t 2) Credit account");
                        Console.WriteLine("\t 3) Deposit account");
                        number = Convert.ToInt32(Console.ReadLine());
                        string? fourthChoice = null;

                        switch (number)
                        {
                            case 1:
                                fourthChoice = "Debit";
                                break;

                            case 2:
                                fourthChoice = "Credit";
                                break;

                            case 3:
                                fourthChoice = "Deposit";
                                break;
                        }

                        switch (fourthChoice!.ToLower())
                        {
                            case "debit":
                                chosenBank.AddNewDebitAccountToClient(client);
                                break;
                            case "credit":
                                chosenBank.AddNewCreditAccountToClient(client);
                                break;
                            case "deposit":
                                Console.WriteLine("Enter the amount of deposit");
                                string? amount = Console.ReadLine();
                                Console.WriteLine("Enter count of years of deposit account");
                                string? yearsOfDepositAccount = Console.ReadLine();
                                chosenBank.AddNewDepositAccountToClient(client, Convert.ToDecimal(amount), DateTime.Now.AddYears(Convert.ToInt32(yearsOfDepositAccount)));
                                break;
                        }
                    }
                    else if (thirdChoice!.ToLower() == "bank accounts")
                    {
                        Console.Clear();
                        foreach (IBankAccount bankaccount in client.BankAccounts)
                        {
                            Console.WriteLine("\t" + count + ")" + " " + bankaccount.AccountId.BankId + " " + bankaccount.AccountId.AccountId + bankaccount.Balance + "$");
                            count++;
                        }

                        Console.WriteLine("Choose bank account");

                        int indexOfBankAccount = Convert.ToInt32(Console.ReadLine());

                        if (!transfer)
                        {
                            bankAccountFrom = client.BankAccounts.ToList()[indexOfBankAccount - 1];
                        }
                        else
                        {
                            bankAccountTo = client.BankAccounts.ToList()[indexOfBankAccount - 1];
                        }

                        Console.WriteLine("MENU:");
                        Console.WriteLine("\t 1)Top up your account");
                        Console.WriteLine("\t 2)Withdraw from the account");
                        Console.WriteLine("\t 3)Transfer to another bank account");

                        number = Convert.ToInt32(Console.ReadLine());
                        string? fivethChoice = null;

                        switch (number)
                        {
                            case 1:
                                fivethChoice = "Top up";
                                break;

                            case 2:
                                fivethChoice = "Withdraw";
                                break;

                            case 3:
                                fivethChoice = "Transfer";
                                break;
                        }

                        if (fivethChoice!.ToLower() == "top up")
                        {
                            Console.WriteLine("Enter the amount of money");
                            decimal amountOfMoney = Convert.ToDecimal(Console.ReadLine());
                            bankAccountFrom!.Bank.MoneyTopUpTransaction(bankAccountFrom!, amountOfMoney);
                        }
                        else if (fivethChoice!.ToLower() == "withdraw")
                        {
                            Console.WriteLine("Enter the amount of money");
                            decimal amountOfMoney = Convert.ToDecimal(Console.ReadLine());
                            bankAccountFrom!.Bank.MoneyWithdrawTransaction(bankAccountFrom!, amountOfMoney);
                        }
                        else if (fivethChoice!.ToLower() == "transfer" && transfer)
                        {
                            Console.WriteLine("Enter the amount of money");
                            decimal amountOfMoney = Convert.ToDecimal(Console.ReadLine());
                            centralBank.MoneyTransferTransaction(bankAccountFrom!, bankAccountTo!, amountOfMoney);
                            bankAccountFrom = null;
                            bankAccountTo = null;
                            transfer = false;
                        }
                        else if (fivethChoice!.ToLower() == "transfer" && !transfer)
                        {
                            Console.WriteLine("Choose another account");
                            Thread.Sleep(2000);
                            transfer = true;
                        }
                    }
                    else if (thirdChoice!.ToLower() == "back")
                    {
                        secondChoice = null;
                        Console.Clear();
                    }

                    Console.Clear();
                }

                if (secondChoice!.ToLower() == "register")
                {
                    string? firstname = null;
                    string? secondname = null;

                    while (firstname == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Enter your firstname");
                        firstname = Console.ReadLine();
                    }

                    while (secondname == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Enter your secondname");
                        secondname = Console.ReadLine();
                    }

                    Console.Clear();
                    Console.WriteLine("Enter your address");
                    Console.WriteLine("or skip, press enter");
                    string? address = Console.ReadLine();

                    Console.Clear();
                    Console.WriteLine("Enter your passport");
                    Console.WriteLine("or skip, press enter");
                    string? passport = Console.ReadLine();

                    if (passport != null && address != null)
                    {
                        Client newClient = new Client.Builder().SetFirstName(firstname!).SetSurname(secondname!).SetAddress(address).SetPassport(passport).Build();
                        var mediator = new Mediator(newClient);
                        chosenBank.AddNewClient(newClient);
                    }
                    else if (passport != null)
                    {
                        Client newClient = new Client.Builder().SetFirstName(firstname!).SetSurname(secondname!).SetPassport(passport).Build();
                        var mediator = new Mediator(newClient);
                        chosenBank.AddNewClient(newClient);
                    }
                    else if (address != null)
                    {
                        Client newClient = new Client.Builder().SetFirstName(firstname!).SetSurname(secondname!).SetAddress(address).Build();
                        var mediator = new Mediator(newClient);
                        chosenBank.AddNewClient(newClient);
                    }

                    Console.WriteLine("Registration is completed");
                    Console.Clear();
                }
                else if (secondChoice!.ToLower() == "transactions")
                {
                    count = 1;

                    Console.Clear();
                    foreach (ITransaction transaction in chosenBank.Transactions)
                    {
                        Console.WriteLine("\t" + count + ")" + " " + transaction.AccountFrom.Owner.Name + " " + transaction.AccountFrom.Owner.Surname
                            + "-" + transaction.AccountTo.Owner.Name + " " + transaction.AccountTo.Owner.Surname + "-" + transaction.Money);
                        count++;
                    }

                    Console.WriteLine("Choose transaction");

                    int indexOfTransaction = Convert.ToInt32(Console.ReadLine());

                    ITransaction chosentransaction = chosenBank.Transactions.ToList()[indexOfTransaction - 1];

                    Console.WriteLine("Do you want to roll it back?");
                    string? sixthChoice = Console.ReadLine();

                    if (sixthChoice!.ToLower() == "yes")
                    {
                        chosentransaction.Rollback();
                    }

                    Console.Clear();
                }
            }
            else if (firstChoice!.ToLower() == "add new bank")
            {
                Console.WriteLine("\tEnter: Name");
                string? name = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("\tEnter: PercentForDebitAccounts");
                decimal percentForDebitAccounts = Convert.ToDecimal(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("\tEnter: PercentLessFiftyThousand");
                decimal percentLessFiftyThousand = Convert.ToDecimal(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("\tEnter: PercentLessOneHundredThousand");
                decimal percentLessOneHundredThousand = Convert.ToDecimal(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("\tEnter: PercentMoreOneHundredThousand");
                decimal percentMoreOneHundredThousand = Convert.ToDecimal(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("\tEnter: CommissionForCreditAccount");
                decimal commissionForCreditAccount = Convert.ToDecimal(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("\tEnter: RestrictionForNotVerifiedCustomers");
                decimal restrictionForNotVerifiedCustomers = Convert.ToDecimal(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("\tEnter: creditLimit");
                decimal creditLimit = Convert.ToDecimal(Console.ReadLine());
                Console.Clear();
                centralBank.AddNewBank(name!, percentForDebitAccounts, percentLessFiftyThousand, percentLessOneHundredThousand, percentMoreOneHundredThousand, commissionForCreditAccount, restrictionForNotVerifiedCustomers, creditLimit);
                Console.Clear();
            }
            else if (firstChoice!.ToLower() == "time")
            {
                Console.WriteLine("\tEnter: months");
                int months = Convert.ToInt32(Console.ReadLine());
                var timeProverForChangingDate = new TimeProvider();
                timeProverForChangingDate.ChangeDate(timeProverForChangingDate.Date.AddMonths(months));
                centralBank.ChangeDate(timeProverForChangingDate.Date);
                Console.Clear();
            }
        }
    }
}