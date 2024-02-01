using Contracts;
using Entities;

namespace Repository;
public class RepositoryManager : IRepositoryManager
{
    private RepositoryContext _context;
    private IAgentRepository _agentRepository;
    private ICustomerRepository _customerRepository;
    private ILoyaltyCustomersRepository _loyaltyCustomersRepository;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;
    }

    public IAgentRepository Agent 
    {
        get
        {
            if(_agentRepository == null)
                _agentRepository = new AgentRepository(_context);

            return _agentRepository;
        }
    }

    public ICustomerRepository Customer 
    {
        get
        {
            if(_customerRepository == null)
                _customerRepository = new CustomerRepository(_context);

            return _customerRepository;
        }
    }

    public ILoyaltyCustomersRepository LoyaltyCustomers 
    {
        get
        {
            if(_loyaltyCustomersRepository == null)
                _loyaltyCustomersRepository = new LoyaltyCustomersRepository(_context);

            return _loyaltyCustomersRepository;
        }
    }

    public Task SaveAsync() => _context.SaveChangesAsync();
}
