using Entities;

namespace Contracts;
public interface ICustomerService
{
    Task ImportSourceCustomers(string connectionString);
    Task ImportCsvCustomers(Stream file, string connectionString);
    Task<bool> UpdateCustomersTable(string connectionString, string updateFrom);
    Task<LoyaltyCustomer> CreateLoyaltyCustomer(LoyaltyCustomerForCreate loyaltyCustomer, string token);
    Task<IEnumerable<CustomerDto>> GetCustomersList();
    Task<CustomerDto> GetCustomerById(Guid customerId);
    Task<CustomerDto> GetCustomerBySsn(string customerSsn);

}
