using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Api;
using WebMvc.Api.Dtos;
using WebMvc.Identity;
using WebMvc.Models.ViewModels;
using WebMvc.Services;

namespace WebMvc.Controllers;

[AllowAnonymous]
public sealed class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ITodoApiClient api;
    private readonly ILogger<AccountController> logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITodoApiClient api,
        ILogger<AccountController> logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.api = api;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        this.ViewData["ReturnUrl"] = returnUrl;
        return this.View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        this.ViewData["ReturnUrl"] = returnUrl;

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var normalized = model.UsernameOrEmail.Trim();
        var user = await this.userManager.FindByNameAsync(normalized) ?? await this.userManager.FindByEmailAsync(normalized);
        if (user is null)
        {
            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return this.View(model);
        }

        var result = await this.signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            this.ModelState.AddModelError(string.Empty, result.IsLockedOut ? "Account locked. Try again later." : "Invalid login attempt.");
            return this.View(model);
        }


        // Ensure API user mapping exists and refresh cookie claims (e.g., ApiUserId).
        await this.TryProvisionApiUserAsync(user);
        await this.signInManager.RefreshSignInAsync(user);

        if (!string.IsNullOrWhiteSpace(returnUrl) && this.Url.IsLocalUrl(returnUrl))
        {
            return this.Redirect(returnUrl);
        }

        return this.RedirectToAction(actionName: "Index", controllerName: "Lists");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return this.View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Username.Trim(),
            Email = model.Email.Trim(),
            EmailConfirmed = true,
        };

        var result = await this.userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.View(model);
        }

        await this.TryProvisionApiUserAsync(user);

        await this.signInManager.SignInAsync(user, isPersistent: false);
        return this.RedirectToAction(actionName: "Index", controllerName: "Lists");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await this.signInManager.SignOutAsync();
        return this.RedirectToAction(actionName: "Login", controllerName: "Account");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return this.View();
    }

    private async Task TryProvisionApiUserAsync(ApplicationUser user)
    {
        try
        {
            var apiUsers = await this.api.GetUsersAsync();
            var existing = apiUsers.FirstOrDefault(u => string.Equals(u.Email, user.Email, StringComparison.OrdinalIgnoreCase));
            if (existing is not null)
            {
                user.ApiUserId = existing.Id;
                _ = await this.userManager.UpdateAsync(user);
                return;
            }

            var newApiId = IdAllocator.NextId(apiUsers.Select(u => u.Id));
            var dto = new UserDto
            {
                Id = newApiId,
                Username = user.UserName ?? user.Email ?? $"user{newApiId}",
                Email = user.Email ?? $"user{newApiId}@local",
                PasswordHash = $"local:{user.Id}",
                CreatedDate = DateTime.UtcNow,
            };

            await this.api.AddUserAsync(dto);

            user.ApiUserId = newApiId;
            _ = await this.userManager.UpdateAsync(user);
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "API error while provisioning API user for {Email}", user.Email);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error while provisioning API user for {Email}", user.Email);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out while provisioning API user for {Email}", user.Email);
        }
    }
}
