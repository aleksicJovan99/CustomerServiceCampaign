using Entities;

namespace Contracts;
public interface ICustomerService
{
    Task ImportSourceCustomers(string connectionString);
    Task<bool> UpdateCustomersTable(string connectionString);
    Task<LoyaltyCustomer> CreateLoyaltyCustomer(LoyaltyCustomerForCreate loyaltyCustomer, string token);
    Task<IEnumerable<CustomerDto>> GetCustomersList();
}
