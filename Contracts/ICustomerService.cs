namespace Contracts;
public interface ICustomerService
{
    Task ImportSourceCustomers(string connectionString);
    Task<bool> UpdateCustomersTable(string connectionString);
}
