using FloreriaAPI_ASP.NET.Models;

namespace FloreriaAPI_ASP.NET.Services;

public class FlowerService : IFlowerService
{
    protected IFlowerRepository _flowersRepo;
    protected ILogger _logger;
    public FlowerService(IFlowerRepository flowersRepo, ILogger logger)
    {
        _flowersRepo = flowersRepo;
        _logger = logger;
    }
    public void isValidFlower(Flower f, bool ignoreOptional = false)
    {
        // throw errors por dato
    }
    public async Task<Flower?> CreateFlower(Flower f)
    {
        Flower? nF = null;
        
        isValidFlower(f);

        try
        {
            nF = await _flowersRepo.CreateFlower(f);
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

    public async Task<List<Flower>> GetAllFlores()
    {
        return await _flowersRepo.GetAllFlowers();
    }

    public async Task<Flower?> GetFlower(int id)
    {
        return await _flowersRepo.GetFlower(id);
    }

    public async Task<Flower> UpdateFlower(int id, Flower f)
    {
        Flower? fExist = await GetFlower(id) ?? throw new Exception("No existe la flor que se intenta modificar");

        isValidFlower(f);

        f = await _flowersRepo.UpdateFlower(id, f);

        return f;
    }
}