using FloreriaAPI_ASP.NET;

public class PermisosEnum
{
    public const string PermisoType = "Permiso";
    public const string FlowerRead = "FlowerRead";
    public const string FlowerCreate = "FlowerCreate";
    public const string FlowerDelete = "FlowerDelete";
    public const string FlowerUpdate = "FlowerUpdate";
    public static IEnumerable<string> Todos = new[]
    {
        FlowerRead,
        FlowerCreate,
        FlowerDelete,
        FlowerUpdate
    };
}