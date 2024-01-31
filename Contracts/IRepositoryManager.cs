namespace Contracts;
public interface IRepositoryManager
{
    IAgentRepository Agent {get; }
    Task SaveAsync();
}
