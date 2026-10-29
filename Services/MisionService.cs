using DSW_3P_PR01.Models;
using DSW_3P_PR01.Repositories;

namespace DSW_3P_PR01.Services;

public class MisionService(IMisionRepository repository) : IMisionService
{
    private static readonly string[] Dificultades = ["Fácil", "Media", "Difícil"];

    public List<Mision> ObtenerTodas() => repository.ObtenerTodas();
    public Mision? ObtenerPorId(int id) => repository.ObtenerPorId(id);

    public Mision Agregar(Mision mision)
    {
        Validar(mision);
        mision.Id = 0;
        mision.Completada = false;
        return repository.Agregar(mision);
    }

    public bool Actualizar(int id, Mision mision)
    {
        Validar(mision);
        return repository.Actualizar(id, mision);
    }

    public bool Eliminar(int id) => repository.Eliminar(id);

    private static void Validar(Mision? mision)
    {
        if (mision is null) throw new ArgumentException("Los datos de la misión son obligatorios.");
        if (string.IsNullOrWhiteSpace(mision.Nombre))
            throw new ArgumentException("El nombre de la misión es obligatorio.");
        if (mision.Dificultad is null || !Dificultades.Contains(mision.Dificultad.Trim(), StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("La dificultad debe ser Fácil, Media o Difícil.");
        if (mision.RecompensaXP < 0)
            throw new ArgumentException("La recompensa XP no puede ser negativa.");

        mision.Nombre = mision.Nombre.Trim();
        mision.Dificultad = Dificultades.First(d => string.Equals(d, mision.Dificultad.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
