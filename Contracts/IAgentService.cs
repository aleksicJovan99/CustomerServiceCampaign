using Entities;

namespace Contracts;
public interface IAgentService
{
    Task<IEnumerable<AgentDto>> GetAgentsList();
    Task<AgentDto> GetAgentById(Guid agentId);
    Task<AgentDto> GetAgentBySsn(string agentSsn);

    Task<AgentForCreateDto?> CreateAgent(AgentForCreateDto agent);
}
