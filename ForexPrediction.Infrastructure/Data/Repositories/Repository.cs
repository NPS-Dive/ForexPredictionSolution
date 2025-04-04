using ForexPrediction.Domain.Interfaces;
using System.Data.Entity; // Use EF6 namespace
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForexPrediction.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ForexDbContext _context;
    private readonly DbSet<T> _entities;

    public Repository ( ForexDbContext context )
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public Task<T> GetByIdAsync ( int id )
    {
        return Task.Run(() => _entities.Find(id));
    }

    public Task<IEnumerable<T>> GetAllAsync ()
    {
        return Task.Run(() => _entities.ToList() as IEnumerable<T>);
    }

    public Task AddAsync ( T entity )
    {
        _entities.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync ( T entity )
    {
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync ( T entity )
    {
        _entities.Remove(entity);
        return Task.CompletedTask;
    }
}