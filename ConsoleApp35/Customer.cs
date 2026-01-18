namespace ConsoleApp35;

public class Customer
{
    public int Id;
    public int Items;
    public bool queueing;
    public Register reg = null;
    public int TimeQueueing;
    public int TimeSpent;
    public bool beingServed;
    public int RemainingCheckoutTime;


    public Customer(int id, int timeSpent, int items, int timeQueueing, bool queueing)
    {
        Id = id;
        TimeSpent = timeSpent;
        Items = items;
        TimeQueueing = timeQueueing;
        this.queueing = queueing;
    }
}