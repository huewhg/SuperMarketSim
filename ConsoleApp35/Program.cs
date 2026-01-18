namespace ConsoleApp35;

internal class Program
{
    private static void Main(string[] args)
    {
        //zacatek AI sekce
        // Reasonable defaults
        const int DEFAULT_ARRIVE_INTERVAL = 20;
        const int DEFAULT_SIM_LENGTH = 1000;
        const int DEFAULT_MAX_NEW = 30;
        const int DEFAULT_MIN_NEW = 5;
        const int DEFAULT_NUM_REGISTERS = 5;
        const int DEFAULT_TIME_PER_ITEM = 1;
        const int DEFAULT_MAX_ITEMS = 10;
        const double DEFAULT_LEAVE_PROB = 0.5;

        Console.WriteLine("Use default settings? (Y/N)");
        var choice = Console.ReadLine();
        var useDefaults = choice != null &&
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
        double leaveProbability;
        var outp = "";

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
                Console.WriteLine("Please input the interval in which batches of customers will arrive (minutes):");
                outp = Console.ReadLine();
            }

            outp = "";
            while (!int.TryParse(outp, out simLength))
            {
                Console.WriteLine("Please input the amount of time to simulate (hours):");
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
                Console.WriteLine("Please input the time per scanning of one item (minutes):");
                outp = Console.ReadLine();
            }

            outp = "";
            while (!int.TryParse(outp, out maxItems))
            {
                Console.WriteLine("Please input the maximum amount items for one customer to get. -1 for infinite:");
                outp = Console.ReadLine();
            }

            outp = "";
            while (!double.TryParse(outp, out leaveProbability))
            {
                Console.WriteLine("Please input the probability for a customer to leave during waiting:");
                outp = Console.ReadLine();
            }
        }

        List<int> allTimesQ = new List<int>();
        List<int> allTimesS = new List<int>();
        int allcheckedOut = new int();
        for (int i = 0; i < 10; i++)
        {
            var (timesQueueing, timesSpent, checkedOut) = NewMethod(simLength, numRegisters, leaveProbability,
                timePerItemCustomer, arriveInterval, minNew, maxNew, maxItems);
            allTimesQ.AddRange(timesQueueing);
            allTimesS.AddRange(timesSpent);
            allcheckedOut += checkedOut;
        }

        int sumQ = new int();
        int sumS = new int();

        foreach (int t in allTimesQ)
        {
            sumQ += t;
        }

        foreach (int t in allTimesS)
        {
            sumS += t;
        }

        Console.WriteLine(
            $"Checked out {allcheckedOut} customers, which waited for {(double)sumQ / allTimesQ.Count} minutes, and shopped for an avreage of {(double)sumS / allTimesS.Count} minutes.");
    }

    private static (List<int> timesQueueing, List<int> timesSpent, int checkedOut) NewMethod(int simLength,
        int numRegisters,
        double leaveProbability, int timePerItemCustomer, int arriveInterval, int minNew, int maxNew, int maxItems)
    {
        var remTime = simLength * 60;

        var rnd = new Random();
        var customers = new List<Customer>();
        var registers = new List<Register>();
        var left = new List<Customer>();
        var timesQueueing = new List<int>();
        var timesSpent = new List<int>();
        var checkedOut = 0;

        for (var i = 0; i < numRegisters; i++) registers.Add(new Register(i, new List<Customer>()));


        var hourIndex = 0;
        var queueLenSumThisHour = 0;
        var arrivalsThisHour = 0;
        var leaversThisHour = 0;
        var servedThisHour = 0;
        var waitSumThisHour = 0;

        var lastId = 0;
        var elapsedTime = 0;

        while (remTime > 0)
        {
            foreach (var c in customers)
            {
                if (c.queueing)
                {
                    if (!c.beingServed && (double)rnd.Next(1000) / 10 < leaveProbability)
                    {
                        c.reg.Queue.Remove(c);
                        Console.WriteLine($"\x1b[1mCustomer {c.Id} has stopped queueing!\x1b[0m");
                        left.Add(c);
                        leaversThisHour++;
                    }
                }
                else
                {
                    if (c.TimeSpent >= c.Items * timePerItemCustomer)
                    {
                        c.queueing = true;

                        var min = int.MaxValue;
                        var minReg = new Register(0);
                        foreach (var r in registers)
                            if (r.Queue.Count < min)
                            {
                                min = r.Queue.Count;
                                minReg = r;
                            }

                        minReg.Queue.Add(c);
                        c.reg = minReg;

                        Console.WriteLine(
                            $"Customer {c.Id} has started queueing at register {minReg.Id}, which has {minReg.Queue.Count} customers in it!");
                    }
                }
            }


            foreach (var r in registers)
            {
                if (r.Queue.Count > 0)
                {
                    var current = r.Queue[0];


                    if (!current.beingServed)
                    {
                        current.beingServed = true;
                        current.RemainingCheckoutTime = current.Items * timePerItemCustomer;

                        servedThisHour++;
                        waitSumThisHour += current.TimeQueueing;

                        Console.WriteLine(
                            $"Customer {current.Id} started being served at register {r.Id} after waiting {current.TimeQueueing} minute(s).");
                    }


                    current.RemainingCheckoutTime--;


                    if (current.RemainingCheckoutTime <= 0)
                    {
                        current.TimeSpent++;

                        Console.WriteLine(
                            $"Customer {current.Id} has successfully checked out at register {r.Id}. They queued for {current.TimeQueueing}");

                        timesQueueing.Add(current.TimeQueueing);
                        timesSpent.Add(current.TimeSpent);

                        customers.Remove(current);
                        r.Queue.RemoveAt(0);
                        checkedOut++;
                    }
                }
            }


            foreach (var l in left)
                customers.Remove(l);
            left = new List<Customer>();


            var waitingNow = 0;
            foreach (var r in registers)
            {
                waitingNow += r.Queue.Count;
                if (r.Queue.Count > 0 && r.Queue[0].beingServed) waitingNow--;
            }

            queueLenSumThisHour += waitingNow;


            elapsedTime++;

            var arrivedThisMinute = 0;
            remTime = SubstractRemTime(
                1,
                remTime,
                arriveInterval,
                rnd,
                minNew,
                maxNew,
                maxItems,
                customers,
                ref lastId,
                elapsedTime,
                ref arrivedThisMinute
            );

            arrivalsThisHour += arrivedThisMinute;


            if (elapsedTime % 60 == 0)
            {
                hourIndex++;

                var avgWait = servedThisHour > 0 ? (double)waitSumThisHour / servedThisHour : 0.0;
                var avgQueueLen = (double)queueLenSumThisHour / 60.0;

                Console.WriteLine($"\n\x1b[1m=== Hour {hourIndex} summary ===\x1b[0m");
                Console.WriteLine($"Average waiting time (started service this hour): {avgWait:F2} minutes");
                Console.WriteLine($"Average customers waiting in queues: {avgQueueLen:F2}");
                Console.WriteLine($"Arrivals this hour: {arrivalsThisHour}");
                Console.WriteLine($"Left while waiting this hour: {leaversThisHour}\n");


                queueLenSumThisHour = 0;
                arrivalsThisHour = 0;
                leaversThisHour = 0;
                servedThisHour = 0;
                waitSumThisHour = 0;
            }

            Console.WriteLine($"\x1b[1m{remTime} simulation minutes remaining!\x1b[0m");
        }

        var totalSpent = 0;
        foreach (var t in timesSpent) totalSpent += t;
        var totalQueued = 0;
        foreach (var t in timesQueueing) totalQueued += t;

        if (checkedOut > 0)
        {
            Console.WriteLine(
                $"Total time customers spent in queue {totalQueued} minutes\n" +
                $"Total time customers spent in store {totalSpent} minutes\n" +
                $"Avreage time spent queueing {(double)totalQueued / checkedOut} minutes\n" +
                $"Avreage time spent in store {(double)totalSpent / checkedOut} minutes");
        }
        else
        {
            Console.WriteLine("No customers checked out.");
        }

        return (timesQueueing, timesSpent, checkedOut);
    }

    private static int SubstractRemTime(
        int minutes,
        int remTime,
        int arriveInterval,
        Random rnd,
        int minNew,
        int maxNew,
        int maxItems,
        List<Customer> customers,
        ref int lastId,
        int elapsedTime,
        ref int arrivedThisMinute
    )
    {
        arrivedThisMinute = 0;

        for (var i = 0; i < minutes; i++)
        {
            foreach (var c in customers)
            {
                c.TimeSpent++;


                if (c.queueing && !c.beingServed) c.TimeQueueing++;
            }

            if (remTime <= 0) return remTime;


            if (elapsedTime % arriveInterval == 0)
            {
                var incomingCount = rnd.Next(minNew, maxNew + 1);
                arrivedThisMinute += incomingCount;

                for (var j = 0; j < incomingCount; j++)
                {
                    var max = maxItems < 0 ? int.MaxValue : maxItems;

                    var tempCustomer = new Customer(
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
        }

        return remTime;
    }
}