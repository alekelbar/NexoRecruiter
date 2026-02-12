using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Domain.Services.Auth;
using NexoRecruiter.Infrastructure.Services.Auth;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Web.Features.Auth.Controllers
{
    [Route("auth-controller")]
    public class AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, INexoAuthStateProvider nexoAuthStateProvider) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly INexoAuthStateProvider nexoAuthStateProvider = nexoAuthStateProvider;

        // TODO: migrar toda esta lógica a un servicio dedicado, el controller solo debería ser un thin layer que recibe la request, la manda al servicio y devuelve la response, toda la lógica de autenticación debería estar en el servicio, no en el controller

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
                return Redirect($"{AppRoutes.Login}?error=1");

            var signInResult = await _signInManager.PasswordSignInAsync(user!, dto.Password, true, false);
            if (!signInResult.Succeeded)
                return Redirect($"{AppRoutes.Login}?error=2");

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
                return Redirect(AppRoutes.Login);

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