using FloreriaAPI_ASP.NET.Models;

namespace FloreriaAPI_ASP.NET.Filters;

public class FlowerFilter
{
    public string? Nombre { get; set; }
    public Family? Family { get; set; }
    public int Offset { get; set; } = 0;
    public int Amount { get; set; } = 20;
}