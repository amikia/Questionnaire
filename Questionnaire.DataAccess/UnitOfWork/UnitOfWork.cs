using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Questionnaire.DataAccess.Context;
using Questionnaire.DataAccess.DomainRepositories;
using Questionnaire.DataAccess.Models.Abstraction;

namespace Questionnaire.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    IUserRepository User { get; }
}


public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _user;

    public UnitOfWork
        (
            ApplicationDbContext context,
            IUserRepository user

        )
    {
        _context = context;
        _user = user;
    }

    public IUserRepository User => _user;


    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entries = _context.ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdateDate = DateTime.Now;
                ((BaseEntity)entityEntry.Entity).UpdateUser = await _user.GetCurrentUserNameAsync(cancellationToken);

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).InsertDate = DateTime.Now;
                    ((BaseEntity)entityEntry.Entity).InsertUser = await _user.GetCurrentUserNameAsync(cancellationToken);
                }
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}