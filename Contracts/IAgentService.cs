using Entities;

namespace Contracts;
public interface IAgentService
{
    Task<IEnumerable<Agent>> GetAgents();
    Task<Agent> GetAgent();
    Task<AgentForCreateDto> CreateAgent(AgentForCreateDto agent);
}
