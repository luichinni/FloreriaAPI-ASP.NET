using FloreriaAPI_ASP.NET.Models;

namespace FloreriaAPI_ASP.NET.DTOs;

public class FlowerDTO
{
    public int? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public Family? Family { get; set; }
}