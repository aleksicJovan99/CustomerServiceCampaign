using System.Linq.Expressions;
using Contracts;
using Entities;

namespace Repository;
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private RepositoryContext _context;

    public RepositoryBase(RepositoryContext context) {
        _context = context;
    }

    public IQueryable<T> FindAll()
    {
        return _context.Set<T>();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }
    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

}