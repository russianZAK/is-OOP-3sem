namespace Banks.Entities;

public class TimeProvider
{
    public TimeProvider(DateTime? initTime = null)
    {
        Date = initTime ?? DateTime.Now;
    }

    public DateTime Date { get; private set; }

    public void ChangeDate(DateTime dateTime)
    {
        Date = dateTime;
    }
}
