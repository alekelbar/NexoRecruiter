using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Web.Features.Auth.Helpers
{
    /// <summary>
    /// Iniciar una semilla de roles y usuarios 
    /// </summary>
    public class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
        {
            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = new string[] { "Admin", "Recruiter" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleCreationResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleCreationResult.Succeeded)
                    {
                        var errors = string.Join(", ", roleCreationResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"No se pudo crear el rol '{role}': {errors}");
                    }
                }

                var adminEmail = configuration["SeedAdmin:Email"];
                var adminPassword = configuration["SeedAdmin:Password"];
                var adminName = configuration["SeedAdmin:Name"] ?? "Admin";
                var adminJobTitle = configuration["SeedAdmin:JobTitle"] ?? "Administrador";

                if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
                {
                    throw new InvalidOperationException("Falta SeedAdmin:Email o SeedAdmin:Password en configuración.");
                }

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser is null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        IsActive = true,
                        FullName = "Admin user",
                        JobTitle = adminJobTitle
                    };

                    var adminUserCreation = await userManager.CreateAsync(adminUser, adminPassword);

                    if (!adminUserCreation.Succeeded)
                    {
                        var errors = string.Join(", ", adminUserCreation.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"No se pudo crear el usuario admin: {errors}");
                    }
                }
                else if (string.IsNullOrWhiteSpace(adminUser.JobTitle))
                {
                    adminUser.JobTitle = adminJobTitle;
                    var updateResult = await userManager.UpdateAsync(adminUser);
                    if (!updateResult.Succeeded)
                    {
                        var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"No se pudo actualizar JobTitle del usuario admin: {errors}");
                    }
                }

                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    var addRoleOperation = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!addRoleOperation.Succeeded)
                    {
                        var errors = string.Join(", ", addRoleOperation.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"No se pudo asignar rol Admin al usuario: {errors}");
                    }
                }
            }
        }
    }
}
