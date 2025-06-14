using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FloreriaAPI_ASP.NET.Models;
namespace FloreriaAPI_ASP.NET.Repository;

public class FloreriaContext : IdentityDbContext<FloreriaUser>
{
    public DbSet<Flower> Flowers { get; set; }

    public FloreriaContext(DbContextOptions<FloreriaContext> op) : base(op)
    {
    }
}