using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NexoRecruiter.Application.Auth.DTOs;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Web.Features.Auth.Controllers
{
    [Route("auth")]
    public class AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

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

            return LocalRedirect(returnUrl);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout([FromForm] string? returnUrl)
        {
            await _signInManager.SignOutAsync();

            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return Redirect("/login");

            return LocalRedirect(returnUrl);
        }

        [HttpGet("Status")]
        public IActionResult Status()
        {
            return Ok(new { Message = "Status OK" });
        }
    }
}