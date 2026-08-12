using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.DataAccess.Models.Abstraction;

namespace Questionnaire.DataAccess.Models;

public class User : BaseEntity
{
    public required string Username { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PasswordHash { get; set; }

    public string FullName() => Firstname + ' ' + Lastname;
}

public class UserConfiguration : BaseEntityConfigurations<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
    }
}