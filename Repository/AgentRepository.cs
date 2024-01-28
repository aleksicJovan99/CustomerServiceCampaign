using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public class AgentRepository : RepositoryBase<Agent>, IAgentRepository
{
    public AgentRepository(RepositoryContext context) : base(context)
    {
    }

    public void CreateAgent(Agent agent) => Create(agent);

    public void DeleteAgent(Agent agent) => Delete(agent);

    public async Task<Agent> GetAgentAsync(int agentId) =>
        await FindByCondition(d => d.Id.Equals(agentId))
        .SingleOrDefaultAsync();

    public async Task<IEnumerable<Agent>> GetAgentsAsync() =>
        await FindAll()
        .OrderBy(d => d.Id)
        .ToListAsync();
}
