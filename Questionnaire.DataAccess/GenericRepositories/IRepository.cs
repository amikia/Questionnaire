using System.Linq.Expressions;

namespace Questionnaire.DataAccess.GenericRepositories;

public interface IRepository<TEntity> 
{
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<TEntity?> SafeGetAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> CheckDuplicateAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task RemovePermanentAsync(TEntity entity, CancellationToken cancellationToken);
    Task RemovePermanentRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
}