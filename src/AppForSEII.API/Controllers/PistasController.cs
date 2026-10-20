using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.PistaDTOs;
using AppForSEII.API.Data;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class PistasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PistasController> _logger;

        public PistasController(ApplicationDbContext context, ILogger<PistasController> logger)
        {
            _context = context;
            _logger = logger;
        }

 /// Muestra las pistas disponibles para el día actual o la fecha filtrada,
/// permitiendo filtrar por tipo de deporte y fecha (Paso 2 y Flujo Alternativo 1).
     
        [HttpGet]
        [ProducesResponseType(typeof(IList<PistaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPistas(string? deporte, DateTime? fecha)
        {
             if (fecha != null && fecha < DateTime.Today)
                 {
                   ModelState.AddModelError("fecha", "La fecha no puede ser anterior a hoy");
                 _logger.LogError($"{DateTime.Now} Error:La fecha no puede ser anterior a hoy.");
                return BadRequest(new ValidationProblemDetails(ModelState));}

                // Si no se proporciona una fecha, se utiliza la fecha actual
                fecha = fecha == null? DateTime.Today : fecha;
                
                IList<PistaDTO> pistas = await _context.Pistas
                    .Include(p => p.TipoDeporte)
                    .Include(p => p.PistasReservadas)
                        .ThenInclude(pr => pr.Reserva)
                    .Where(p =>
                    (deporte == null || (p.TipoDeporte != null && p.TipoDeporte.Nombre.Contains(deporte)))
                    && (p.PistasReservadas.Where(pr => pr.Reserva != null
                     && pr.Reserva.FechaReserva.Date == fecha.Value.Date).Count() == 0))
                    .Select(p => new PistaDTO
                    {
                        IdPista = p.IdPista,
                        NombrePista = p.NombrePista,
                        NPersonas = p.NPersonas,
                        TipoDeporte = p.TipoDeporte == null ? string.Empty : p.TipoDeporte.Nombre,
                        Precio = p.Precio
                    })
                    .ToListAsync();

    return Ok(pistas);

        }
    }
}