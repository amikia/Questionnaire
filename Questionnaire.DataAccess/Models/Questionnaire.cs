using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.DataAccess.Models.Abstraction;

namespace Questionnaire.DataAccess.Models;

public class Questionnaire : BaseEntity
{
    public required string Title { get; set; }

    public virtual ICollection<Question> Questions { get; set; }
    public virtual ICollection<Submission> Submissions { get; set; }

}

public class QuestionnaireConfiguration : BaseEntityConfigurations<Questionnaire>
{
    public override void Configure(EntityTypeBuilder<Questionnaire> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Questions)
            .WithOne(x => x.Questionnaire)
            .HasForeignKey(x => x.QuestionnaireId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Submissions)
            .WithOne(x => x.Questionnaire)
            .HasForeignKey(x => x.QuestionnaireId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}