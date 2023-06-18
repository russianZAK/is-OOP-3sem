using Banks.Exceptions;
using Banks.Notifications;

namespace Banks.Models;

public class Client : IObserver<INotification>
{
    private List<IBankAccount> _bankAccounts;
    private List<INotification> _spamNotifications;
    private List<INotification> _usefulNotifications;

    public Client(Builder builder)
    {
        Name = builder.FirstName;
        Surname = builder.Surname;
        Address = builder.Address;
        Passport = builder.Passport;
        IsVerified = builder.IsVerified;
        _bankAccounts = new List<IBankAccount>();
        _spamNotifications = new List<INotification>();
        _usefulNotifications = new List<INotification>();
    }

    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Address { get; private set; }
    public string Passport { get; private set; }
    public bool IsVerified { get; private set; }
    public Bank? Bank { get; private set; }
    public IMediator? Mediator { get; private set; }

    public IReadOnlyCollection<IBankAccount> BankAccounts => _bankAccounts;
    public IReadOnlyCollection<INotification> SpamNotifications => _spamNotifications;
    public IReadOnlyCollection<INotification> UsefulNotifications => _usefulNotifications;

    public void AddMediator(IMediator mediator)
    {
        if (mediator == null) throw ClientException.InvalidMediator();
        Mediator = mediator;
    }

    public void AddBankAccount(IBankAccount bankAccount)
    {
        if (bankAccount == null) throw ClientException.InvalidBankAccount();
        _bankAccounts.Add(bankAccount);
    }

    public void AddBank(Bank bank)
    {
        if (bank == null) throw ClientException.InvalidBank();
        Bank = bank;
    }

    public void Update(INotification payload)
    {
        if (payload == null) throw ClientException.InvalidPayload();

        Mediator!.Notify(this, payload);
    }

    public void AddSpamNotification(INotification payload)
    {
        if (payload == null) throw ClientException.InvalidPayload();

        _spamNotifications.Add(payload);
    }

    public void AddUsefulNotification(INotification payload)
    {
        if (payload == null) throw ClientException.InvalidPayload();

        _usefulNotifications.Add(payload);
    }

    public class Builder
    {
        public Builder()
        {
            FirstName = string.Empty;
            Surname = string.Empty;
            Address = string.Empty;
            Passport = string.Empty;
        }

        public string FirstName { get; private set; }
        public string Surname { get; private set; }
        public string Address { get; private set; }
        public string Passport { get; private set; }
        public bool IsVerified { get; private set; }

        public Builder SetFirstName(string firstName)
        {
            if (firstName == null) throw ClientException.InvalidData();
            FirstName = firstName;
            return this;
        }

        public Builder SetSurname(string surname)
        {
            if (surname == null) throw ClientException.InvalidData();

            Surname = surname;
            return this;
        }

        public Builder SetAddress(string address)
        {
            if (address == null) throw ClientException.InvalidData();

            Address = address;
            return this;
        }

        public Builder SetPassport(string passport)
        {
            if (passport == null) throw ClientException.InvalidData();

            Passport = passport;
            return this;
        }

        public Client Build()
        {
            if (Address != string.Empty && Passport != string.Empty)
            {
                IsVerified = true;
            }

            return new Client(this);
        }
    }
}
