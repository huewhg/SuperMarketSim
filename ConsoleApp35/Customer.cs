namespace ConsoleApp35;

public class Customer
{
    public int Id;
    public int TimeSpent;
    public int Items;
    public int TimeQueueing;
    public bool queueing = false;
    public Register reg = null;


    public Customer(int id, int timeSpent, int items, int timeQueueing, bool queueing)
    {
        Id = id;
        TimeSpent = timeSpent;
        Items = items;
        TimeQueueing = timeQueueing;
        this.queueing = queueing;
    }
}