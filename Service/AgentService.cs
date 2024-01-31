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
        
        // Check if an agent with the same SSN already exists
        var agentCheck = await _repository.Agent.GetAgentBySsnAsync(agentDto.Ssn);

        if (agentCheck != null) return null;
        
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

        var result = _mapper.Map<AgentDto>(agent);

        return result;
    }

    // Method to get agent by SSN number
    public async Task<AgentDto> GetAgentBySsn(string agentSsn)
    {
        var agent = await _repository.Agent.GetAgentBySsnAsync(agentSsn);

        if (agent == null) return null;

        var result = _mapper.Map<AgentDto>(agent);

        return result;
    }

    // Method to get a list of agents
    public async Task<IEnumerable<AgentDto>> GetAgentsList()
    {
        var agents = await _repository.Agent.GetAgentsAsync();

        if (agents == null) { return null; }

        var result = _mapper.Map<IEnumerable<AgentDto>>(agents);

        return result;
    }
}
