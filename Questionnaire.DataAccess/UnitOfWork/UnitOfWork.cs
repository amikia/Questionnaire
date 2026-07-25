using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Questionnaire.DataAccess.Context;
using Questionnaire.DataAccess.Models.Abstraction;

namespace Questionnaire.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}


public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork
        (
            ApplicationDbContext context

        )
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entries = _context.ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdateDate = DateTime.Now;
                ((BaseEntity)entityEntry.Entity).UpdateUser = "System";

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).InsertDate = DateTime.Now;
                    ((BaseEntity)entityEntry.Entity).InsertUser = "System";
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