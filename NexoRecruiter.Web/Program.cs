using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using NexoRecruiter.Domain.Services.Email.ValueObjects;
using NexoRecruiter.Infrastructure;
using NexoRecruiter.Infrastructure.Persistence;
using NexoRecruiter.Web;
using NexoRecruiter.Web.Features.Auth.Helpers;
using NexoRecruiter.Web.Features.Auth.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<RecruiterDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NexoRecruiterDatabase"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<RecruiterDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/login";

    // Expiración total
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    // Renueva la cookie si el usuario sigue activo
    options.SlidingExpiration = true;

    // IMPORTANTE: no persistir eternamente
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization(opt =>
{
    // Agregar que por defecto TODO requiere autenticación a menos que explicitamente se defina lo contrario
    opt.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddMudServices();

builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:5110/api/");
});

builder.Services.AddControllersWithViews();
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(24)
); // Tokens de reset de contraseña expiran en 24 horas
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddInfraestructure();
var app = builder.Build();

await IdentitySeeder.SeedAsync(app.Services, app.Configuration);

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// 1) Archivos estáticos y mapeo de assets (anónimo)
app.MapStaticAssets().AllowAnonymous();

// 2) Autenticación/Autorización antes de mapear endpoints protegidos
app.UseAuthentication();
app.UseAuthorization();

// 3) Antiforgery una sola vez
app.UseAntiforgery();

// 4) Endpoints
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();
