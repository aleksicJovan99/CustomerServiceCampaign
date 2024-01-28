using Contracts;
using Entities;

namespace Repository;
public class RepositoryManager : IRepositoryManager
{
    private RepositoryContext _context;
    private IAgentRepository _agentRepository;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;
    }

    public IAgentRepository Agent 
    {
        get
        {
            if(_agentRepository == null)
                _agentRepository = new AgentRepository(_context);

            return _agentRepository;
        }
    }

    public Task SaveAsync() => _context.SaveChangesAsync();
}
