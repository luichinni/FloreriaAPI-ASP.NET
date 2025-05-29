namespace FloreriaAPI_ASP.NET.Models;
public interface IFlowerService
{
    protected void isValidFlower(Flower f, bool ignoreOptional = false);
    public Task<Flower?> GetFlower(int id);
    public Task<List<Flower>> GetAllFlores();
    public Task<Flower?> CreateFlower(Flower f);
    public Task<Flower> UpdateFlower(int id, Flower f);
    public Task<bool> DeleteFlower(int id);
}