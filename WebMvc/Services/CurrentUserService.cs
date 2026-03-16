using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WebMvc.Api;
using WebMvc.Identity;

namespace WebMvc.Services;

public sealed class CurrentUserService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ITodoApiClient api;
    private readonly ILogger<CurrentUserService> logger;

    public CurrentUserService(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        ITodoApiClient api,
        ILogger<CurrentUserService> logger)
    {
        this.userManager = userManager;
        this.httpContextAccessor = httpContextAccessor;
        this.api = api;
        this.logger = logger;
    }

    public async Task<ApplicationUser?> GetUserAsync()
    {
        var httpContext = this.httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return null;
        }

        var principal = httpContext.User;
        if (principal.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var user = await this.userManager.GetUserAsync(principal);
        if (user is not null)
        {
            return user;
        }

        var name = principal.Identity?.Name;
        if (!string.IsNullOrWhiteSpace(name))
        {
            user = await this.userManager.FindByNameAsync(name) ?? await this.userManager.FindByEmailAsync(name);
            if (user is not null)
            {
                return user;
            }
        }

        var email = principal.FindFirstValue(ClaimTypes.Email);
        if (!string.IsNullOrWhiteSpace(email))
        {
            user = await this.userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                return user;
            }
        }

        this.logger.LogWarning(
            "Authenticated principal could not be mapped to an ApplicationUser. Claims: {Claims}",
            string.Join(", ", principal.Claims.Select(c => $"{c.Type}={c.Value}")));

        return null;
    }

    public async Task<int?> GetApiUserIdAsync()
    {
        var httpContext = this.httpContextAccessor.HttpContext;
        var principal = httpContext?.User;

        if (principal?.Identity?.IsAuthenticated == true)
        {
            var apiUserIdClaim = principal.FindFirstValue(CustomClaimTypes.ApiUserId);
            if (int.TryParse(apiUserIdClaim, out var apiUserId) && apiUserId > 0)
            {
                return apiUserId;
            }

            // If the identity store is unavailable (in-memory) or ApiUserId claim isn't present,
            // resolve the API user by email claim.
            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrWhiteSpace(email))
            {
                try
                {
                    var users = await this.api.GetUsersAsync();
                    var match = users.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
                    if (match is not null)
                    {
                        return match.Id;
                    }
                }
                catch (TodoApiException ex)
                {
                    this.logger.LogWarning(ex, "Todo API error while resolving ApiUserId for {Email}", email);
                }
                catch (HttpRequestException ex)
                {
                    this.logger.LogWarning(ex, "HTTP error while resolving ApiUserId for {Email}", email);
                }
                catch (TaskCanceledException ex)
                {
                    this.logger.LogWarning(ex, "Todo API request timed out while resolving ApiUserId for {Email}", email);
                }
            }
        }

        var user = await this.GetUserAsync();
        if (user is null || user.ApiUserId <= 0)
        {
            return null;
        }

        return user.ApiUserId;
    }
}
