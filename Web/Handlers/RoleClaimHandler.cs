using FloreriaAPI_ASP.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class RoleClaimHandler : AuthorizationHandler<RoleClaimRequirement>
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<FloreriaUser> _userManager;

    public RoleClaimHandler(RoleManager<IdentityRole> roleManager, UserManager<FloreriaUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleClaimRequirement requirement)
    {
        Console.WriteLine("Entra");
        Console.WriteLine(context.User.Identity?.IsAuthenticated ?? true);
        var user = await _userManager.GetUserAsync(context.User);
        if (user == null)
            return;
        Console.WriteLine("No es null");
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;

            var claims = await _roleManager.GetClaimsAsync(role);

            if (claims.Any(c => c.Type == requirement.ClaimType && c.Value == requirement.ClaimValue))
            {
                Console.WriteLine("Tiene permisos");
                context.Succeed(requirement);
                return;
            }
        }
    }
}