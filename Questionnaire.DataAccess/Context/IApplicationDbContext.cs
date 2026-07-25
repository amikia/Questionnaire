using Microsoft.EntityFrameworkCore;
using Questionnaire.DataAccess.Models;

namespace Questionnaire.DataAccess.Context;

public interface IApplicationDbContext
{
    public DbSet<Answer> Answers { get; }
    public DbSet<Question> Questions { get; }
    public DbSet<QuestionItem> QuestionItems { get; }
    public DbSet<Models.Questionnaire> Questionnaires { get; }
    public DbSet<Respondent> Respondents { get; }
    public DbSet<Submission> Submissions { get; }
    public DbSet<User> Users { get; }  
}