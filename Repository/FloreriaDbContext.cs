using Microsoft.EntityFrameworkCore;
using FloreriaAPI_ASP.NET.Models;
namespace FloreriaAPI_ASP.NET.Repository;

public class FloreriaContext : DbContext
{
    public DbSet<Flower> Flowers { get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("data source=Floreria.sqlite");
    }
}