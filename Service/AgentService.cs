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

    public async Task<AgentForCreateDto> CreateAgent(AgentForCreateDto agentDto)
    {
        var agent = _mapper.Map<Agent>(agentDto);
        var agents = await _repository.Agent.GetAgentsAsync();

        var exist = agents.Any(a => a.Ssn == agent.Ssn);

        if (!exist) 
        {
            _repository.Agent.CreateAgent(agent);
            await _repository.SaveAsync();
            return agentDto;
        }
        else
        {
            return new AgentForCreateDto();
        }

       
    }

    public Task<Agent> GetAgent()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Agent>> GetAgents()
    {
        var agents = await _repository.Agent.GetAgentsAsync();

        return agents;
    }
}
