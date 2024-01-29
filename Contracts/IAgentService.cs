using Entities;

namespace Contracts;
public interface IAgentService
{
    Task<IEnumerable<Agent>> GetAgentsList();
    Task<Agent> GetAgent();
    Task<AgentForCreateDto?> CreateAgent(AgentForCreateDto agent);
}
