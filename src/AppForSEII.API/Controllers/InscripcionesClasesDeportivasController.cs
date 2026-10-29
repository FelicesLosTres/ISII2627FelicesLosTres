using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ClaseDeportivaDTOs;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionClasesDeportivasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InscripcionClasesDeportivasController> _logger;

        public InscripcionClasesDeportivasController(ApplicationDbContext context, ILogger<InscripcionClasesDeportivasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Muestra las clases deportivas disponibles para inscribirse (Paso 2 del CU4).
        /// Permite filtrar por tipo de deporte, monitor y fecha (por defecto hoy).
        /// Verifica que existan plazas disponibles (PlazasDisponibles > 0).
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<InscripcionClaseDeportivaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetClasesParaInscripcion(string? tipodeporte, string? monitor, DateTime? fecha)
        {
            // Validación de regla de negocio: la fecha no puede ser anterior al día actual
            if (fecha != null && fecha.Value.Date < DateTime.Today)
            {
                ModelState.AddModelError("fecha", "La fecha no puede ser anterior a hoy");
                _logger.LogError($"{DateTime.Now} Error: La fecha no puede ser anterior a hoy.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Si no se proporciona fecha, se utiliza la fecha de hoy
            DateTime fechaFiltro = fecha?.Date ?? DateTime.Today;

            IList<InscripcionClaseDeportivaDTO> clases = await _context.ClasesDeportivas
                .Include(c => c.TipoDeporte)
                .Where(c =>
                    // 1. Coincidencia de fecha con el día filtrado
                    c.FechaHora.Date == fechaFiltro.Date

                    // 2. Solo clases con plazas libres disponibles
                    && c.PlazasDisponibles > 0

                    // 3. Filtro opcional por tipo de deporte
                    && (string.IsNullOrEmpty(tipodeporte) || 
                        (c.TipoDeporte != null && c.TipoDeporte.Nombre.Contains(tipodeporte)))

                    // 4. Filtro opcional por monitor
                    && (string.IsNullOrEmpty(monitor) || 
                        c.Monitor.Contains(monitor))
                )
                .Select(c => new InscripcionClaseDeportivaDTO(
                    c.IdClaseDeportiva,
                    c.FechaHora,
                    c.Lugar,
                    c.Monitor,
                    c.Nivel,
                    c.PlazasDisponibles,
                    c.PrecioUnitario,
                    c.Nombre,
                    c.Descripcion,
                    c.Duracion,
                    c.TipoDeporte != null ? c.TipoDeporte.Nombre : string.Empty
                ))
                .ToListAsync();
            return Ok(clases);
        }
    }
}