using AutoMapper;
using Contracts;
using Entities;

namespace Service;
public class AgentService : IAgentService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public AgentService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AgentForCreateDto?> CreateAgent(AgentForCreateDto agentDto)
    {
        var agentCheck = await _repository.Agent.GetAgentBySsnAsync(agentDto.Ssn);

        if (agentCheck == null) 
        {
            var agentMapped = _mapper.Map<Agent>(agentDto);
            _repository.Agent.CreateAgent(agentMapped);
            await _repository.SaveAsync();

            return agentDto;
        }
        else
        {
            return null;
        }

       
    }

    public Task<Agent> GetAgent()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Agent>> GetAgentsList()
    {
        var agents = await _repository.Agent.GetAgentsAsync();

        return agents;
    }
}
