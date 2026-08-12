using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Questionnaire.DataAccess.Models.Abstraction;

public interface IBaseEntity
{
}

public abstract class BaseEntity : IBaseEntity 
{
    public required Guid Id { get; set; } = new Guid();
    public DateTime InsertDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; }
    public string InsertUser { get; set; } = "System";
    public string UpdateUser { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}

public abstract class BaseEntityConfigurations<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}