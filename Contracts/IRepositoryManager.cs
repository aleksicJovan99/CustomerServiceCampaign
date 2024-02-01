namespace Contracts;
public interface IRepositoryManager
{
    IAgentRepository Agent {get; }
    ICustomerRepository Customer {get; }
    Task SaveAsync();
}
