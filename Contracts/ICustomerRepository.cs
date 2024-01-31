using Entities;

namespace Contracts;
public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(Guid customerId);
    Task<Customer> GetCustomerBySsnAsync(string customerSsn);
    void CreateCustomer(Customer customer);
    void DeleteCustomer(Customer customer);
}
