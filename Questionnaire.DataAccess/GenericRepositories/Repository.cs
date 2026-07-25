using Microsoft.EntityFrameworkCore;
using Questionnaire.DataAccess.Context;
using Questionnaire.DataAccess.Models.Abstraction;
using System.Linq.Expressions;

namespace Questionnaire.DataAccess.GenericRepositories;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationDbContext _context = context;

    public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().FindAsync([id], cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity?> SafeGetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> CheckDuplicateAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().CountAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);

        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        entity.IsDeleted = true;
        _context.Set<TEntity>().Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
        }

        _context.Set<TEntity>().UpdateRange(entities);

        return Task.CompletedTask;
    }

    public Task RemovePermanentAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public Task RemovePermanentRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().RemoveRange(entities);

        return Task.CompletedTask;
    }
}