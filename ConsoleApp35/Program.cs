namespace ConsoleApp35;

class Program
{
    static void Main(string[] args)
    {
        int arriveInterval;
        int simLength;
        DateTime startTime;
        DateTime endTime;
        int maxNew;
        int minNew;
        int numRegisters;
        int timePerItem;
        int maxItems;
        int leaveProbability;
        int timePerItemCustomer = 3;
        string outp = "";
        while (!int.TryParse(outp, out arriveInterval))
        {
            Console.WriteLine("Please input the interval in which batches of customers will arrive:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out simLength))
        {
            Console.WriteLine("Please input the amount of time to simulate:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out maxNew))
        {
            Console.WriteLine("Please input the maximum amount of customers to arrive at one time:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out minNew))
        {
            Console.WriteLine("Please input the minimum amount of customers to arrive at one time:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out numRegisters))
        {
            Console.WriteLine("Please input the amount of registers the simulated store has:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out timePerItemCustomer))
        {
            Console.WriteLine("Please input the time per scanning of one item:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out maxItems))
        {
            Console.WriteLine("Please input the maximum amount items for one customer to get. -1 for infinite:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out leaveProbability))
        {
            Console.WriteLine("Please input the probability for a customer to leave during waiting:");
            outp = Console.ReadLine();
        }

        int remTime = simLength;
        Random rnd = new Random();
        List<Customer> customers = new List<Customer>();
        int lastId = 0;
        while (remTime > 0)
        {
            int timeInStep = 0;
            foreach (Customer c in customers)
            {
                c.TimeSpent++;
                if (c.queueing)
                    c.TimeQueueing++;
                else
                {
                    if (c.TimeSpent > c.Items * timePerItemCustomer)
                    {
                        c.queueing = true;
                        Console.WriteLine($"Customer {c.Id} has started queueing!");
                    }
                    else
                    {
                        timeInStep += timePerItemCustomer;
                        Console.WriteLine(
                            $"Customer {c.Id} has {(int)(c.TimeSpent / timePerItemCustomer)} out of {c.Items} items!");
                    }
                }
            }

            remTime = SubstractRemTime(timeInStep, remTime, arriveInterval, rnd, minNew, maxNew, maxItems, customers,
                ref lastId);
            Console.WriteLine($"\x1b[1m{remTime} simulation time remaining!\x1b[0m");
        }
    }

    private static int SubstractRemTime(int minutes, int remTime, int arriveInterval, Random rnd, int minNew,
        int maxNew,
        int maxItems, List<Customer> customers, ref int lastId)
    {
        if (remTime <= 0) return remTime;

        if (remTime % arriveInterval == 0)
        {
            int incomingCount = rnd.Next(minNew, maxNew + 1);

            for (int j = 0; j < incomingCount; j++)
            {
                int max;
                if (maxItems < 0) max = int.MaxValue;
                else max = maxItems;

                Customer tempCustomer = new Customer(
                    lastId + 1,
                    0,
                    rnd.Next(0, maxItems),
                    0,
                    false
                );

                lastId = tempCustomer.Id;
                customers.Add(tempCustomer);
            }

            Console.WriteLine($"{incomingCount} customers were added to the store!");
        }

        remTime--;

        return remTime;
    }
}