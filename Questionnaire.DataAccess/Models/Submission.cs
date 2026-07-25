using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.DataAccess.Models.Abstraction;

namespace Questionnaire.DataAccess.Models;

public class Submission : BaseEntity
{
    public required Guid QuestionnaireId { get; set; }
    public required Guid RespondentId { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.Now;

    public virtual Questionnaire Questionnaire { get; set; }
    public virtual Respondent Respondent { get; set; }
    public virtual ICollection<Answer> Answers { get; set; }
}

public class SubmissionConfiguration : BaseEntityConfigurations<Submission>
{
    public override void Configure(EntityTypeBuilder<Submission> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Questionnaire)
            .WithMany(x => x.Submissions)
            .HasForeignKey(x => x.QuestionnaireId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Respondent)
            .WithMany(x => x.Submissions)
            .HasForeignKey(x => x.RespondentId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}