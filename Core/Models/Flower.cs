namespace FloreriaAPI_ASP.NET.Models;

public class Flower
{
    public int Id {get; set;}
    public string? Nombre {get; set;}
    public string? Descripcion {get; set;}
    public Familia Familia {get; set;}
}