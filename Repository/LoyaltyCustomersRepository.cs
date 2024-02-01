using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public class LoyaltyCustomersRepository : RepositoryBase<LoyaltyCustomer>, ILoyaltyCustomersRepository
{
    public LoyaltyCustomersRepository(RepositoryContext context) : base(context)
    {
    }

    public void CreateCustomer(LoyaltyCustomer customer) => Create(customer);

    public void DeleteCustomer(LoyaltyCustomer customer) => Delete(customer);

    public async Task<LoyaltyCustomer>GetCustomerByIdAsync(Guid customerId) =>
         await FindByCondition(d => d.CustomerId.Equals(customerId)).SingleOrDefaultAsync();


    public async Task<IEnumerable<LoyaltyCustomer>> GetCustomersAsync() => await FindAll().ToListAsync();
}
