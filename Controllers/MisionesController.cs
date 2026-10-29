using DSW_3P_PR01.Models;
using DSW_3P_PR01.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MisionesController(IMisionService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Mision>), StatusCodes.Status200OK)]
    public ActionResult<List<Mision>> ObtenerTodas() => Ok(service.ObtenerTodas());

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Mision> ObtenerPorId(int id)
    {
        var mision = service.ObtenerPorId(id);
        return mision is null ? NotFound() : Ok(mision);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Mision> Agregar([FromBody] Mision mision)
    {
        try
        {
            var creada = service.Agregar(mision);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (ArgumentException ex) { return BadRequest(new { mensaje = ex.Message }); }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Actualizar(int id, [FromBody] Mision mision)
    {
        try { return service.Actualizar(id, mision) ? NoContent() : NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { mensaje = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id) => service.Eliminar(id) ? NoContent() : NotFound();
}
