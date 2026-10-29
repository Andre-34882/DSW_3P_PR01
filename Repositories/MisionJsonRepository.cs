using System.Text.Json;
using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

public class MisionJsonRepository : IMisionRepository
{
    private readonly string _rutaArchivo;
    private readonly object _lock = new();
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public MisionJsonRepository(IWebHostEnvironment environment)
    {
        var directorio = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(directorio);
        _rutaArchivo = Path.Combine(directorio, "misiones.json");
        if (!File.Exists(_rutaArchivo))
            File.WriteAllText(_rutaArchivo, "[]");
    }

    public List<Mision> ObtenerTodas()
    {
        lock (_lock) return Leer();
    }

    public Mision? ObtenerPorId(int id)
    {
        lock (_lock) return Leer().FirstOrDefault(m => m.Id == id);
    }

    public Mision Agregar(Mision mision)
    {
        lock (_lock)
        {
            var misiones = Leer();
            mision.Id = misiones.Count == 0 ? 1 : misiones.Max(m => m.Id) + 1;
            misiones.Add(mision);
            Guardar(misiones);
            return mision;
        }
    }

    public bool Actualizar(int id, Mision mision)
    {
        lock (_lock)
        {
            var misiones = Leer();
            var indice = misiones.FindIndex(m => m.Id == id);
            if (indice < 0) return false;
            mision.Id = id;
            misiones[indice] = mision;
            Guardar(misiones);
            return true;
        }
    }

    public bool Eliminar(int id)
    {
        lock (_lock)
        {
            var misiones = Leer();
            var mision = misiones.FirstOrDefault(m => m.Id == id);
            if (mision is null) return false;
            misiones.Remove(mision);
            Guardar(misiones);
            return true;
        }
    }

    private List<Mision> Leer()
    {
        var json = File.ReadAllText(_rutaArchivo);
        return JsonSerializer.Deserialize<List<Mision>>(json) ?? [];
    }

    private void Guardar(List<Mision> misiones)
    {
        var temporal = _rutaArchivo + ".tmp";
        File.WriteAllText(temporal, JsonSerializer.Serialize(misiones, JsonOptions));
        File.Move(temporal, _rutaArchivo, overwrite: true);
    }
}
