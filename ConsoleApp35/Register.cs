namespace ConsoleApp35;

public class Register
{
    public List<Customer> Queue;
    public int Id;


    public Register(int id, List<Customer> queue = null)
    {
        this.Id = id;
        Queue = queue ?? new List<Customer>();
    }
}