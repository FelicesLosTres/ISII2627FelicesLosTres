using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ClaseDeportivaDTOs;
using AppForSEII.API.Data;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClasesDeportivasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClasesDeportivasController> _logger;

        public ClasesDeportivasController(ApplicationDbContext context, ILogger<ClasesDeportivasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Muestra las clases deportivas disponibles para el día actual o la fecha filtrada,
        /// permitiendo filtrar por tipo de deporte y monitor.
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ClaseDeportivaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetClasesDeportivas(string? tipodeporte, string? monitor, DateTime? fecha)
        {
            if (fecha != null && fecha < DateTime.Today)
            {
                ModelState.AddModelError("fecha", "La fecha no puede ser anterior a hoy");
                _logger.LogError($"{DateTime.Now} Error:La fecha no puede ser anterior a hoy.");
                return NotFound(new ValidationProblemDetails(ModelState));
            }

            var fechaFiltro = fecha?.Date ?? DateTime.Today;

            IList<ClaseDeportivaDTO> clases = await _context.ClasesDeportivas
                .Include(c => c.TipoDeporte)
                .Where(c =>
                    c.FechaHora.Date == fechaFiltro
                    && c.PlazasDisponibles > 0
                    && (tipodeporte == null || c.TipoDeporte != null && c.TipoDeporte.Nombre.Contains(tipodeporte))
                    && (monitor == null || c.Monitor.Contains(monitor))
                )
                .Select(c => new ClaseDeportivaDTO( c.IdClaseDeportiva,c.FechaHora, c.Lugar,c.Monitor,c.Nivel,c.PlazasDisponibles,
                 c.PrecioUnitario,c.Nombre,c.Descripcion,c.Duracion,
                 c.TipoDeporte == null ? string.Empty : c.TipoDeporte.Nombre
                ))
                .ToListAsync();

            if (clases.Count == 0)
            {
                ModelState.AddModelError("clases", "No se encontraron clases deportivas ");
                _logger.LogError($"{DateTime.Now} Error: No se encontraron clases deportivas");
                return NotFound(new ValidationProblemDetails(ModelState));
            }

            return Ok(clases);
        }
    }
}
