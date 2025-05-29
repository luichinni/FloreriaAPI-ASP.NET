using FloreriaAPI_ASP.NET.DTOs;
using FloreriaAPI_ASP.NET.Filters;
using FloreriaAPI_ASP.NET.Models;

namespace FloreriaAPI_ASP.NET.Services;

public class FlowerService : IFlowerService
{
    protected IFlowerRepository _flowersRepo;
    protected ILogger<FlowerService> _logger;
    public FlowerService(IFlowerRepository flowersRepo, ILogger<FlowerService> logger)
    {
        _flowersRepo = flowersRepo;
        _logger = logger;
    }
    public void isValidFlower(Flower f, bool ignoreOptional = false)
    {
        // throw errors por dato
    }
    public async Task<Flower?> CreateFlower(FlowerDTO f)
    {
        Flower? nF = toFlower(f);
        
        isValidFlower(nF);

        try
        {
            nF = await _flowersRepo.CreateFlower(nF);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CreateFlower en Service");
        }

        return nF;
    }

    public async Task<bool> DeleteFlower(int id)
    {
        bool ok = false;

        Flower? f = await GetFlower(id) ?? throw new Exception("No existe la flor que se intenta eliminar");
        
        try
        {
            ok = await _flowersRepo.DeleteFlower(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "DeleteFlower en Service");
        }

        return ok;
    }

    public async Task<List<Flower>> GetAllFlowers(FlowerFilter filter)
    {
        return await _flowersRepo.GetAllFlowers(filter);
    }

    public async Task<Flower?> GetFlower(int id)
    {
        return await _flowersRepo.GetFlower(id);
    }

    public async Task<Flower> UpdateFlower(int id, FlowerDTO f)
    {
        _logger.LogInformation("Entrando update service");
        Flower? fExist = await GetFlower(id) ?? throw new Exception("No existe la flor que se intenta modificar");

        Flower nFlower = toFlower(f);

        isValidFlower(nFlower);

        _logger.LogInformation("Entrando al try");

        try
        {
            return await _flowersRepo.UpdateFlower(id, nFlower);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Update Flower");
        }
        return nFlower;
    }

    public Flower toFlower(FlowerDTO fd)
    {
        return new Flower()
        {
            Nombre = fd.Nombre,
            Descripcion = fd.Descripcion,
            Family = fd.Family.HasValue ? fd.Family.Value : Family.Otro
        };
    }
}