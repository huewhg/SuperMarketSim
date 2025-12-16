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
        while (!int.TryParse(outp, out arriveInterval))
        {
            Console.WriteLine("Please input the interval in which batches of customers will arrive:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out numRegisters))
        {
            Console.WriteLine("Please input the amount of registers the simulated store has:");
            outp = Console.ReadLine();
        }

        outp = "";
        while (!int.TryParse(outp, out timePerItem))
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
            if (remTime % arriveInterval == 0)
            {
                for (int i = 0; i < rnd.Next(minNew, maxNew + 1); i++)
                {
                    Customer tempCustomer = new Customer(lastId + 1, 0, rnd.Next(0, maxItems + 1), 0, false);
                    lastId = tempCustomer.Id;
                }
            }
        }
    }
}