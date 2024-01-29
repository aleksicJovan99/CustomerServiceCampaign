using AutoMapper;
using Contracts;
using Entities;
using Microsoft.Extensions.Configuration;

namespace Service;
public class AgentService : IAgentService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AgentService(IRepositoryManager repository, IMapper mapper, IConfiguration configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
    }


    public async Task<AgentForCreateDto?> CreateAgent(AgentForCreateDto agentDto)
    {
        var encryptedSsn = EncryptHelper.EncryptDataBase64(agentDto.Ssn);
        var agentCheck = await _repository.Agent.GetAgentBySsnAsync(encryptedSsn);

        if (agentCheck == null) 
        {
            agentDto.Ssn = encryptedSsn;
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

        var result = new List<Agent>();

        foreach(var agent in agents)
        {
            agent.Ssn = EncryptHelper.DecryptDataBase64(agent.Ssn);
            result.Add(agent);
        }

        return agents;
    }
}
