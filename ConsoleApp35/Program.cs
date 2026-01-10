namespace ConsoleApp35;

class Program
{
    static void Main(string[] args)
    {
        //zacatek AI sekce
        // Reasonable defaults
        const int DEFAULT_ARRIVE_INTERVAL = 5;
        const int DEFAULT_SIM_LENGTH = 60;
        const int DEFAULT_MAX_NEW = 5;
        const int DEFAULT_MIN_NEW = 1;
        const int DEFAULT_NUM_REGISTERS = 5;
        const int DEFAULT_TIME_PER_ITEM = 3;
        const int DEFAULT_MAX_ITEMS = 30;
        const int DEFAULT_LEAVE_PROB = 50;


        Console.WriteLine("Use default settings? (Y/N)");
        string? choice = Console.ReadLine();
        bool useDefaults = choice != null &&
                           choice.Trim().StartsWith("Y", StringComparison.OrdinalIgnoreCase);
        //konec AI sekce
        int arriveInterval;
        int simLength;
        DateTime startTime;
        DateTime endTime;
        int maxNew;
        int minNew;
        int numRegisters;
        int timePerItemCustomer;
        int maxItems;
        int leaveProbability;
        string outp = "";
        //zacatek AI sekce
        if (useDefaults)
        {
            arriveInterval = DEFAULT_ARRIVE_INTERVAL;
            simLength = DEFAULT_SIM_LENGTH;
            maxNew = DEFAULT_MAX_NEW;
            minNew = DEFAULT_MIN_NEW;
            numRegisters = DEFAULT_NUM_REGISTERS;
            timePerItemCustomer = DEFAULT_TIME_PER_ITEM;
            maxItems = DEFAULT_MAX_ITEMS;
            leaveProbability = DEFAULT_LEAVE_PROB;
        }
        else //konec AI sekce
        {
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
        }

        int remTime = simLength;
        Random rnd = new Random();
        List<Customer> customers = new List<Customer>();
        List<Register> registers = new List<Register>();
        List<Customer> left = new List<Customer>();

        for (int i = 0; i < numRegisters; i++)
        {
            registers.Add(new Register(i, new List<Customer>()));
        }

        int lastId = 0;
        while (remTime > 0)
        {
            int timeInStep = 0;
            foreach (Customer c in customers)
            {
                c.TimeSpent++;
                if (c.queueing)
                {
                    c.TimeQueueing++;
                    if (rnd.Next(100) < leaveProbability)
                    {
                        c.reg.Queue.Remove(c);
                        Console.WriteLine($"\x1b[1mCustomer {c.Id} has stopped queueing!\x1b[0m");
                        left.Add(c);
                    }
                }
                else
                {
                    if (c.TimeSpent >= c.Items * timePerItemCustomer)
                    {
                        c.queueing = true;
                        int min = int.MaxValue;
                        Register minReg = new Register(0);
                        foreach (Register r in registers)
                        {
                            if (r.Queue.Count < min)
                            {
                                min = r.Queue.Count;
                                minReg = r;
                            }
                        }

                        minReg.Queue.Add(c);
                        c.reg = minReg;
                        Console.WriteLine(
                            $"Customer {c.Id} has started queueing at register {minReg.Id}, which has {minReg.Queue.Count} customers in it!");
                    }
                    else
                    {
                        timeInStep += timePerItemCustomer;
                        Console.WriteLine(
                            $"Customer {c.Id} has {(int)(c.TimeSpent / timePerItemCustomer)} out of {c.Items} items!");
                    }
                }
            }

            foreach (Register r in registers)

            {
                Console.WriteLine($"First in line at register {r.Id}");
                if (r.Queue.Count > 0) Console.WriteLine(r.Queue[0].Id);
                Console.WriteLine($"Customers in line at register {r.Id}: {r.Queue.Count}");
            }

            foreach (Customer l in left)
                customers.Remove(l);

            left = new List<Customer>();


            remTime = SubstractRemTime(timeInStep, remTime, arriveInterval, rnd, minNew, maxNew, maxItems, customers,
                ref lastId);
            Console.WriteLine($"\x1b[1m{remTime} simulation time remaining!\x1b[0m");
        }
    }

    private static int SubstractRemTime(int minutes, int remTime, int arriveInterval, Random rnd, int minNew,
        int maxNew, int maxItems, List<Customer> customers, ref int lastId)
    {
        if (remTime <= 0) return remTime;

        if (remTime % arriveInterval == 0)
        {
            int incomingCount = rnd.Next(minNew, maxNew + 1);

            for (int j = 0; j < incomingCount; j++)
            {
                int max = maxItems < 0 ? int.MaxValue : maxItems;

                Customer tempCustomer = new Customer(
                    lastId + 1,
                    0,
                    rnd.Next(0, max),
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