using Microsoft.EntityFrameworkCore;
using FloreriaAPI_ASP.NET.Models;
using FloreriaAPI_ASP.NET.Filters;

namespace FloreriaAPI_ASP.NET.Repository;

public class FlowerRepository : IFlowerRepository
{
    private FloreriaContext _context;
    private ILogger<FlowerRepository> _logger;
    public FlowerRepository(FloreriaContext context, ILogger<FlowerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<Flower> CreateFlower(Flower f)
    {
        await _context.AddAsync(f);
        await _context.SaveChangesAsync();
        return f;
    }

    public async Task<bool> DeleteFlower(int id)
    {
        bool ok = false;
        Flower? f = await GetFlower(id);

        if (f is not null)
        {
            _context.Remove(f);
            ok = true;
        }

        return ok;
    }

    public async Task<List<Flower>> GetAllFlowers(FlowerFilter filter)
    {
        var query = _context.Flowers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Nombre))
            query = query.Where(f => f.Nombre == filter.Nombre);

        if (filter.Family.HasValue)
            query = query.Where(f => f.Family == filter.Family);

        return  (await query.ToListAsync())
                    .Skip(filter.Offset)
                    .Take(filter.Amount)
                    .ToList();
    }

    public Task<Flower?> GetFlower(int id)
    {
        return _context.Flowers.SingleOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Flower> UpdateFlower(int id, Flower f)
    {
        Flower? original = await GetFlower(id) ?? throw new Exception("No existe la id de flor para actualizar");

        if (f.Nombre != original.Nombre)
            original.Nombre = f.Nombre;

        if (f.Descripcion != original.Descripcion)
            original.Descripcion = f.Descripcion;

        if (f.Family != original.Family)
            original.Family = f.Family;

        await _context.SaveChangesAsync();

        return original;
    }
}