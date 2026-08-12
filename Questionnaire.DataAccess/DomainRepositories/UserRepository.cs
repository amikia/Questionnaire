using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Questionnaire.DataAccess.Context;
using Questionnaire.DataAccess.GenericRepositories;
using Questionnaire.DataAccess.Models;
using System.Security.Claims;

namespace Questionnaire.DataAccess.DomainRepositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);
    Task<string> GetCurrentUserNameAsync(CancellationToken cancellationToken);
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
}

public class UserRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : Repository<User>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException();
        }

        var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        return user is null ? throw new UnauthorizedAccessException() : user;
    }

    public async Task<string> GetCurrentUserNameAsync(CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException();
        }

        var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        return user is null ? throw new UnauthorizedAccessException() : user.Username;
    }

    public async Task<User?> GetByUserNameAsync(string username, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(x => x.Username == username && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
