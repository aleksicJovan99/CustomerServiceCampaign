using Entities;

namespace Contracts;
public interface IAgentRepository
{
    Task<IEnumerable<Agent>> GetAgentsAsync();
    Task<Agent> GetAgentAsync(int agentId);
    void CreateAgent(Agent director);
    void DeleteAgent(Agent agent);
}
