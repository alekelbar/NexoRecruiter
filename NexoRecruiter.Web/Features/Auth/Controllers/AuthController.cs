using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Services.Auth;
using NexoRecruiter.Infrastructure.Services.Auth;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Web.Features.Auth.Controllers
{
    [Route("auth")]
    public class AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, INexoAuthStateProvider nexoAuthStateProvider) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly INexoAuthStateProvider nexoAuthStateProvider = nexoAuthStateProvider;

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password, [FromForm] string? returnUrl)
        {
            returnUrl = (returnUrl.IsNullOrEmpty() || !Url.IsLocalUrl(returnUrl)) ? "/dashboard" : returnUrl;

            var dto = new LoginDTO()
            {
                Email = email,
                Password = password
            };

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Redirect("/login?error=1");

            var signInResult = await _signInManager.PasswordSignInAsync(user!, dto.Password, true, false);
            if (!signInResult.Succeeded)
                return Redirect("/login?error=2");

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Notify the authentication state provider about the change in authentication state
            (nexoAuthStateProvider as NexoAuthStateProvider)?.NotifyAuthenticationStateChanged(HttpContext.User);
            return LocalRedirect(returnUrl);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout([FromForm] string? returnUrl)
        {
            await _signInManager.SignOutAsync();

            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return Redirect("/login");

            // if the implementation change should notify the authentication state provider about the change in authentication state, this is not needed, but for safety we can do it here as well
            (nexoAuthStateProvider as NexoAuthStateProvider)?.NotifyAuthenticationStateChanged(null);
            return LocalRedirect(returnUrl);
        }

        [HttpGet("Status")]
        public IActionResult Status()
        {
            return Ok(new { Message = "Status OK" });
        }
    }
}