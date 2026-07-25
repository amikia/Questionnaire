using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.DataAccess.Models.Abstraction;
using Questionnaire.SharedKernel.Enums;

namespace Questionnaire.DataAccess.Models;

public class Question : BaseEntity
{
    public required Guid QuestionnaireId { get; set; }
    public required string Text { get; set; }
    public bool IsRequired { get; set; }
    public QuestionType Type { get; set; }

    public virtual Questionnaire Questionnaire { get; set; }
    public virtual ICollection<Answer> Answers { get; set; }
    public virtual ICollection<QuestionItem> QuestionItems { get; set; }
}

public class QuestionConfiguration : BaseEntityConfigurations<Question>
{
    public override void Configure(EntityTypeBuilder<Question> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Questionnaire)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.QuestionnaireId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}