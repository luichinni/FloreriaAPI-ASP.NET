using FloreriaAPI_ASP.NET.Filters;

namespace FloreriaAPI_ASP.NET.Models;
public interface IFlowerRepository
{
    public abstract Task<Flower?> GetFlower(int id);
    public Task<List<Flower>> GetAllFlowers(FlowerFilter filter);
    public Task<Flower> CreateFlower(Flower f);
    public Task<Flower> UpdateFlower(int id, Flower f);
    public Task<bool> DeleteFlower(int id);
}