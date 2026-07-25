using Microsoft.EntityFrameworkCore;
using Questionnaire.DataAccess.Models;

namespace Questionnaire.DataAccess.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionItem> QuestionItems { get; set; }
    public DbSet<Models.Questionnaire> Questionnaires { get; set; }
    public DbSet<Respondent> Respondents { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<User> Users{ get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }
}