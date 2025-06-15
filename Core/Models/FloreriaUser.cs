using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FloreriaAPI_ASP.NET.Models;

public class FloreriaUser : IdentityUser
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? DNI { get; set; }
    public FloreriaUser() : base()
    {

    }
}