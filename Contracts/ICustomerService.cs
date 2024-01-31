namespace Contracts;
public interface ICustomerService
{
    Task GetSourceCustomers(string connectionString);
}
