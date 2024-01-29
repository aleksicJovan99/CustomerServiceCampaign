using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Contracts;
using Entities;

namespace Service;

// Service class responsible for handling agent-related operations
public class AgentService : IAgentService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public AgentService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // Method to create a new agent
    public async Task<AgentForCreateDto?> CreateAgent(AgentForCreateDto agentDto)
    {
        // Encrypt the SSN using EncryptHelper
        var encryptedSsn = EncryptHelper.EncryptDataBase64(agentDto.Ssn);

        // Check if an agent with the same SSN already exists
        var agentCheck = await _repository.Agent.GetAgentBySsnAsync(encryptedSsn);

        if (agentCheck != null) return null;
        
        agentDto.Ssn = encryptedSsn;
        var agentMapped = _mapper.Map<Agent>(agentDto);
        _repository.Agent.CreateAgent(agentMapped);
        await _repository.SaveAsync();

        return agentDto;
        
    }

    // Method to get agent by ID
    public async Task<AgentDto> GetAgentById(Guid agentId)
    {
        var agent = await _repository.Agent.GetAgentByIdAsync(agentId);

        if (agent == null) return null;

        agent.Ssn = EncryptHelper.DecryptDataBase64(agent.Ssn);
        var result = _mapper.Map<AgentDto>(agent);

        return result;
    }

    // Method to get agent by SSN number
    public async Task<AgentDto> GetAgentBySsn(string agentSsn)
    {
        var encryptedSsn = EncryptHelper.EncryptDataBase64(agentSsn);
        var agent = await _repository.Agent.GetAgentBySsnAsync(encryptedSsn);

        if (agent == null) return null;

        agent.Ssn = EncryptHelper.DecryptDataBase64(agent.Ssn);
        var result = _mapper.Map<AgentDto>(agent);

        return result;
    }

    // Method to get a list of agents with decrypted SSN
    public async Task<IEnumerable<AgentDto>> GetAgentsList()
    {
        var agents = await _repository.Agent.GetAgentsAsync();

        if (agents == null) { return null; }

        var result = new List<AgentDto>();

        // Decrypt the SSN property of each agent and add it to the result list
        foreach(var agent in agents)
        {
            agent.Ssn = EncryptHelper.DecryptDataBase64(agent.Ssn);
            var agentMapped = _mapper.Map<AgentDto>(agent);

            result.Add(agentMapped);
        }

        return result;
    }
}
