namespace ConsoleApp35;

public class Register
{
    public int Id;
    public List<Customer> Queue;


    public Register(int id, List<Customer> queue = null)
    {
        Id = id;
        Queue = queue ?? new List<Customer>();
    }
}