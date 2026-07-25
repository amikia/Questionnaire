using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.DataAccess.Models.Abstraction;

namespace Questionnaire.DataAccess.Models;

public class Answer : BaseEntity
{
    public required Guid SubmissionId { get; set; }
    public required Guid QuestionId { get; set; }
    public string? TextAnswer { get; set; }
    public int? NumberAnswer { get; set; }
    public DateTime? DateAnswer { get; set; }

    public virtual Submission Submission { get; set; }
    public virtual Question Question { get; set; }
}

public class AnswerConfiguration : BaseEntityConfigurations<Answer>
{
    public override void Configure(EntityTypeBuilder<Answer> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Question)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Submission)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.SubmissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}