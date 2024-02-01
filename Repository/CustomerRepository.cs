using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(RepositoryContext context) : base(context)
    {
    }

    public void CreateCustomer(Customer customer) => Create(customer);

    public void DeleteCustomer(Customer customer) => Delete(customer);

    public async Task<Customer>GetCustomerByIdAsync(Guid customerId) =>
         await FindByCondition(d => d.Id.Equals(customerId)).SingleOrDefaultAsync();

    public async Task<Customer> GetCustomerBySsnAsync(string customerSsn) =>
        await FindByCondition(d => d.SSN.Equals(customerSsn)).SingleOrDefaultAsync();


    public async Task<IEnumerable<Customer>> GetCustomersAsync() => await FindAll().ToListAsync();

}
