using Entities;

namespace Contracts;
public interface IAgentRepository
{
    Task<IEnumerable<Agent>> GetAgentsAsync();
    Task<Agent> GetAgentByIdAsync(Guid agentId);
    Task<Agent> GetAgentBySsnAsync(string agentSsn);
    void CreateAgent(Agent director);
    void DeleteAgent(Agent agent);
}
