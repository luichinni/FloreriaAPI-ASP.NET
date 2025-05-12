using FloreriaAPI_ASP.NET.Models;

namespace FloreriaAPI_ASP.NET.Services;

public class FlorService{
    static List<Flor> Flores {get;}
    static int nextId = 4;
    static FlorService()
    {
        Flores = new List<Flor>
        {
            new Flor { Id = 1, Nombre = "Rosa Roja", Descripcion = "La del principito", Familia = Familia.Rosaceas },
            new Flor { Id = 2, Nombre = "Lirio", Descripcion = "Lirililarila", Familia = Familia.Liliaceas},
            new Flor { Id = 3, Nombre = "Tulipan", Descripcion = "Como los preservativos", Familia = Familia.Liliaceas}
        };
    }

    public static List<Flor> GetAll() => Flores;

    public static Flor? Get(int id) => Flores.FirstOrDefault(p => p.Id == id);

    public static void Add(Flor flor)
    {
        flor.Id = nextId++;
        Flores.Add(flor);
    }

    public static void Delete(int id)
    {
        var flor = Get(id);
        if (flor is null)
            return;

        Flores.Remove(flor);
    }

    public static void Update(Flor flor)
    {
        var index = Flores.FindIndex(p => p.Id == flor.Id);
        if (index == -1)
            return;

        Flores[index] = flor;
    }
}