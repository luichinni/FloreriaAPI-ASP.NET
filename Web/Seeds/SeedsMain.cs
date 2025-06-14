using System.Security.Claims;
using FloreriaAPI_ASP.NET.Models;
using FloreriaAPI_ASP.NET.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FloreriaAPI_ASP.NET.Web.Seeds;

public static class Seeds
{
    public static async Task Run(WebApplication app)
    {
        // Inicializo rol admin y cuenta de admin con todos los permisos existentes.
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<FloreriaContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<FloreriaUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            IdentityRole? adminRol = await roleManager.FindByNameAsync("Admin");

            if (adminRol is null)
            {
                adminRol = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRol);
            }

            var claims = await roleManager.GetClaimsAsync(adminRol);

            if (claims.Count != PermisosEnum.Todos.Count())
            {
                foreach (var permiso in PermisosEnum.Todos)
                {
                    if (!claims.Any(c => c.Value == permiso))
                        await roleManager.AddClaimAsync(adminRol, new Claim(PermisosEnum.PermisoType, permiso));
                }
            }

            var user = await userManager.FindByEmailAsync("admin@floreria.com");
            if (user is null)
            {
                user = new FloreriaUser
                {
                    UserName = "admin",
                    Email = "admin@floreria.com",
                    Nombre = "Admin",
                    Apellido = "istrador",
                    DNI = "44200300"
                };
                await userManager.CreateAsync(user, "Admin123!");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}