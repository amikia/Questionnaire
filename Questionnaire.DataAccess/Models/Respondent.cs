using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.DataAccess.Models.Abstraction;

namespace Questionnaire.DataAccess.Models;

public class Respondent : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }

    public virtual ICollection<Submission> Submissions { get; set; }
}

public class RespondentConfiguration : BaseEntityConfigurations<Respondent>
{
    public override void Configure(EntityTypeBuilder<Respondent> builder)
    {
        base.Configure(builder);
    }
}