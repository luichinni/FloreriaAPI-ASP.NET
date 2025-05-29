using FloreriaAPI_ASP.NET.DTOs;
using FloreriaAPI_ASP.NET.Filters;

namespace FloreriaAPI_ASP.NET.Models;
public interface IFlowerService
{
    protected Flower toFlower(FlowerDTO fd);
    protected void isValidFlower(Flower f, bool ignoreOptional = false);
    public Task<Flower?> GetFlower(int id);
    public Task<List<Flower>> GetAllFlowers(FlowerFilter filter);
    public Task<Flower?> CreateFlower(FlowerDTO f);
    public Task<Flower> UpdateFlower(int id, FlowerDTO f);
    public Task<bool> DeleteFlower(int id);
}