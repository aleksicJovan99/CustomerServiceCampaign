using Entities;

namespace Contracts;
public interface ILoyaltyCustomersRepository
{
    Task<IEnumerable<LoyaltyCustomer>> GetCustomersAsync();
    Task<LoyaltyCustomer> GetCustomerByIdAsync(Guid customerId);
    void CreateCustomer(LoyaltyCustomer customer);
    void DeleteCustomer(LoyaltyCustomer customer);
}
