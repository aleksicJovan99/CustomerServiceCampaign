namespace Contracts;
public interface IRepositoryManager
{
    IAgentRepository Agent {get; }
    ICustomerRepository Customer {get; }
    ILoyaltyCustomersRepository LoyaltyCustomers {get; }
    Task SaveAsync();
}
