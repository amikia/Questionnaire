using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.DataAccess.Models.Abstraction;

namespace Questionnaire.DataAccess.Models;

public class QuestionItem : BaseEntity
{
    public required Guid QuestionId { get; set; }
    public required string Text { get; set; }
    public int Order { get; set; }

    public virtual Question Question { get; set; } = null!;
}

public class QuestionItemConfiguration : BaseEntityConfigurations<QuestionItem>
{
    public override void Configure(EntityTypeBuilder<QuestionItem> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Question)
            .WithMany(x => x.QuestionItems)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}