using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Application.Services.Session;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Web.Features.Auth.Controllers
{
    [Route("auth-controller")]
    public class AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISessionService sessionService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ISessionService _sessionService = sessionService;

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password, [FromForm] string? returnUrl)
        {
            returnUrl = (returnUrl.IsNullOrEmpty() || !Url.IsLocalUrl(returnUrl)) ? AppRoutes.Dashboard : returnUrl;

            var dto = new LoginDTO()
            {
                Email = email,
                Password = password
            };

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Redirect($"{AppRoutes.Login}?error={AppLoginResults.UserNotFound}");

            if (!user.EmailConfirmed)
                return Redirect($"{AppRoutes.Login}?error={AppLoginResults.EmailNotConfirmed}");

            if (!user.IsActive)
                return Redirect($"{AppRoutes.Login}?error={AppLoginResults.InactiveAccount}");

            var signInResult = await _signInManager.PasswordSignInAsync(user!, dto.Password, true, false);
            if (!signInResult.Succeeded)
                return Redirect($"{AppRoutes.Login}?error={AppLoginResults.InvalidPassword}");

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            if (_sessionService is SessionService sessionServiceImpl)
                await sessionServiceImpl.NotifyAuthenticationStateChangedAsync(HttpContext.User);
            return LocalRedirect(returnUrl);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout([FromForm] string? returnUrl)
        {
            await _signInManager.SignOutAsync();

            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return Redirect(AppRoutes.Login);

            if (_sessionService is SessionService sessionServiceImpl)
                await sessionServiceImpl.NotifyAuthenticationStateChangedAsync(null);
            return LocalRedirect(returnUrl);
        }

        [HttpGet("Status")]
        public IActionResult Status()
        {
            return Ok(new { Message = "Status OK" });
        }
    }
}