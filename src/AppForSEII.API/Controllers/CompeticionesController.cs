using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.CompeticionDTOs;
using AppForSEII.API.Data;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompeticionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompeticionesController> _logger;

        public CompeticionesController(ApplicationDbContext context, ILogger<CompeticionesController> logger)
        {
            _context = context;
            _logger = logger;
        }

      
        /// GET: api/Competiciones?lugar=...&deporte=...
       
        [HttpGet]
        [ProducesResponseType(typeof(List<CompeticionDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CompeticionDTO>>> GetCompeticiones(
            [FromQuery] string? lugar,
            [FromQuery] string? deporte,
            CancellationToken cancellationToken)
        {
            var query = _context.Competiciones
                .AsNoTracking()
                .Include(c => c.TipoDeporte)
                .Include(c => c.CompeticionesInscritas)
                .Where(c => c.Plazas > c.CompeticionesInscritas.Count);

            if (!string.IsNullOrWhiteSpace(lugar))
            {
                query = query.Where(c => c.Lugar.Contains(lugar));
            }

            if (!string.IsNullOrWhiteSpace(deporte))
            {
                query = query.Where(c =>
                    c.TipoDeporte != null && c.TipoDeporte.Nombre.Contains(deporte));
            }

            var resultado = await query
                .OrderBy(c => c.Fecha)
                .ThenBy(c => c.Nombre)
                .Select(c => new CompeticionDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    TipoDeporte = c.TipoDeporte == null ? string.Empty : c.TipoDeporte.Nombre,
                    Fecha = c.Fecha,
                    Lugar = c.Lugar,
                    Precio = c.Precio,
                    PlazasLibres = c.Plazas - c.CompeticionesInscritas.Count
                })
                .ToListAsync(cancellationToken);

            return Ok(resultado);
        }
    }
}
