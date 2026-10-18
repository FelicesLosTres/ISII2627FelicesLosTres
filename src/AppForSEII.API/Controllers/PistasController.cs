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

        // GET: api/Pistas?deporte=...
        [HttpGet]
        [ProducesResponseType(typeof(List<PistaDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PistaDTO>>> GetPistas(
            [FromQuery] string? deporte,
            CancellationToken cancellationToken)
        {
            var query = _context.Pistas
                .AsNoTracking()
                .Include(p => p.TipoDeporte)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(deporte))
            {
                query = query.Where(p =>
                    p.TipoDeporte != null && p.TipoDeporte.Nombre.Contains(deporte));
            }

            var resultado = await query
                .Select(p => new PistaDTO
                {
                    IdPista = p.IdPista,
                    NombrePista = p.NombrePista,
                    NPersonas = p.NPersonas,
                    TipoDeporte = p.TipoDeporte == null ? string.Empty : p.TipoDeporte.Nombre,
                    Precio = p.Precio
                })
                .ToListAsync(cancellationToken);

            return Ok(resultado);
        }
    }
}