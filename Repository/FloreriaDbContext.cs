using Microsoft.EntityFrameworkCore;
using FloreriaAPI_ASP.NET.Models;
namespace FloreriaAPI_ASP.NET.Repository;

public class FloreriaContext : DbContext
{
    public DbSet<Flower> Flowers { get; set; }

    public FloreriaContext(DbContextOptions op) : base(op)
    {
    }
}