using Banks.Exceptions;
using Banks.Models;
using Banks.Notifications;

namespace Banks.Entities;

public class Mediator : IMediator
{
    private Client _client;

    public Mediator(Client client)
    {
        if (client == null) throw MediatorException.InvalidClient();

        _client = client;
    }

    public void Notify(Client client, INotification notification)
    {
        if (client == null) throw MediatorException.InvalidClient();
        if (notification == null) throw MediatorException.InvalidNotification();

        if (notification is ChangeCommissionForCreditAccountNotification)
        {
            string[] message = notification.Message.Split();
            decimal newCommision = Convert.ToDecimal(message[0]);

            if (client.Bank!.CommissionForCreditAccount > newCommision)
            {
                client.AddUsefulNotification(notification);
            }
            else
            {
                client.AddSpamNotification(notification);
            }
        }
        else if (notification is ChangeCreditLimitNotification)
        {
            string[] message = notification.Message.Split();
            decimal newCreditLimit = Convert.ToDecimal(message[0]);

            if (client.Bank!.CreditLimit > newCreditLimit)
            {
                client.AddSpamNotification(notification);
            }
            else
            {
                client.AddUsefulNotification(notification);
            }
        }
        else if (notification is ChangePercentsForDebitAccountNotification)
        {
            string[] message = notification.Message.Split();
            decimal newPercent = Convert.ToDecimal(message[0]);

            if (client.Bank!.PercentForDebitAccounts > newPercent)
            {
                client.AddSpamNotification(notification);
            }
            else
            {
                client.AddUsefulNotification(notification);
            }
        }
        else if (notification is ChangeRestrictionForNotVerifiedCustomersNotification)
        {
            string[] message = notification.Message.Split();
            decimal newRestriction = Convert.ToDecimal(message[0]);

            if (client.Bank!.RestrictionForNotVerifiedCustomers > newRestriction)
            {
                client.AddSpamNotification(notification);
            }
            else
            {
                client.AddUsefulNotification(notification);
            }
        }
        else if (notification is ChangePercentForDepositAccountNotification)
        {
            string[] message = notification.Message.Split();
            string data = message[0].ToString();
            decimal newPercent = Convert.ToDecimal(message[1]);

            if (data == "PercentLessFiftyThousand" && client.Bank!.PercentLessFiftyThousand > newPercent)
            {
                client.AddUsefulNotification(notification);
            }
            else
            {
                client.AddSpamNotification(notification);
            }

            if (data == "PercentLessOneHundredThousand" && client.Bank!.PercentLessOneHundredThousand > newPercent)
            {
                client.AddUsefulNotification(notification);
            }
            else
            {
                client.AddSpamNotification(notification);
            }

            if (data == "PercentMoreOneHundredThousand" && client.Bank!.PercentMoreOneHundredThousand > newPercent)
            {
                client.AddUsefulNotification(notification);
            }
            else
            {
                client.AddSpamNotification(notification);
            }
        }
    }
}
