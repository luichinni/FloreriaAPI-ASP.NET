
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FloreriaAPI_ASP.NET.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FloreriaAPI_ASP.NET.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public record UserDTO
    (
        string User,
        [EmailAddress] string Email,
        string Nombre,
        string Apellido,
        [RegularExpression("^[0-9]*$")] string DNI,
        List<string> Permisos
    );
    public record RegisterDTO
    (
        string User,
        [EmailAddress] string Email,
        string Nombre,
        string Apellido,
        string DNI,
        string Password
    );
    public record LoginDTO
    (
        string Email,
        string Password
    );
    private readonly UserManager<FloreriaUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<FloreriaUser> _signIn;
    public AuthController(UserManager<FloreriaUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<FloreriaUser> signIn)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signIn = signIn;
    }

    [Authorize]
    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        Console.WriteLine("Responde");
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            Name = User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        FloreriaUser? user = await _userManager.FindByEmailAsync(login.Email);

        if (user is null) return Unauthorized(new { Message = "Usuario o contraseña incorrectos." });

        // Esto supuestamente establece la cookie ya configurada en el Program.cs
        var loginResult = await _signIn.PasswordSignInAsync(user, login.Password, isPersistent: true, lockoutOnFailure: false);

        if (loginResult.Succeeded)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);

            List<Claim> roleClaims = new List<Claim>();

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var claims = await _roleManager.GetClaimsAsync(role);
                    roleClaims.AddRange(claims.Where(c => c.Type == "Permiso"));
                }
            }
            return Ok(new UserDTO(
               user.UserName!,
               user.Email!,
               user.Nombre!,
               user.Apellido!,
               user.DNI!,
               roleClaims.Select(p => p.Value).ToList()
            ));
        }
        else if (loginResult.IsNotAllowed) return Forbid();
        else if (loginResult.IsLockedOut) return Unauthorized(new { Message = "No puedes iniciar sesión." });
        else return Unauthorized(new { Message = "Usuario o contraseña incorrectos." });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerData)
    {
        FloreriaUser? existeUser = await _userManager.FindByNameAsync(registerData.User);

        if (existeUser is null) return BadRequest(new { Message = "El usuario ya está en uso, elige otro." });

        FloreriaUser user = new()
        {
            UserName = registerData.User,
            Email = registerData.Email,
            Nombre = registerData.Nombre,
            Apellido = registerData.Apellido,
            DNI = registerData.DNI
        };

        var registerStatus = await _userManager.CreateAsync(user, registerData.Password);

        if (registerStatus.Succeeded)
        {
            var addedToRoleStatus = await _userManager.AddToRoleAsync(user, "Visitante");
            if (!addedToRoleStatus.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return StatusCode(500, new { Message = "Error al asignar rol al usuario." });
            }
            return Ok(new { Message = "Usuario registrado con éxito." });
        }
        return BadRequest(new { Message = "Error al registrar usuario." });
    }
}