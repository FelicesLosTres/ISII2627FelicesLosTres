using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.Data;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.InscripcionCompeticionDTOs;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionesCompeticionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InscripcionesCompeticionesController> _logger;

        public InscripcionesCompeticionesController(ApplicationDbContext context, ILogger<InscripcionesCompeticionesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el detalle de una inscripción a competiciones por su Id (Paso 7)
        /// GET: api/InscripcionesCompeticiones/5
        /// </summary>
        [HttpGet("{idInscripcion:int}")]
        [ProducesResponseType(typeof(InscripcionCompeticionDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<InscripcionCompeticionDetalleDTO>> GetInscripcionById(
            int idInscripcion,
            CancellationToken cancellationToken)
        {
            var inscripcion = await _context.Inscripciones
                .AsNoTracking()
                .Include(i => i.ApplicationUser)
                .Include(i => i.CompeticionesInscritas)
                    .ThenInclude(ci => ci.Competicion)
                        .ThenInclude(c => c!.TipoDeporte)
                .FirstOrDefaultAsync(i => i.Id == idInscripcion && i.CompeticionesInscritas.Any(), cancellationToken);

            if (inscripcion == null)
            {
                _logger.LogWarning("Inscripción a competiciones con Id={IdInscripcion} no encontrada.", idInscripcion);
                return NotFound($"No existe ninguna inscripción a competiciones con la clave {idInscripcion}.");
            }

            var dto = new InscripcionCompeticionDetalleDTO
            {
                IdInscripcion = inscripcion.Id,
                FechaInscripcion = inscripcion.FechaInscripcion,
                PrecioTotal = inscripcion.PrecioTotal,
                MetodoPago = inscripcion.MetodoPago.ToString(),
                NombreCliente = inscripcion.ApplicationUser?.Name ?? string.Empty,
                ApellidosCliente = inscripcion.ApplicationUser?.Surname ?? string.Empty,
                Email = inscripcion.ApplicationUser?.Email ?? string.Empty,
                Telefono = inscripcion.ApplicationUser?.PhoneNumber ?? string.Empty,
                CompeticionesInscritas = inscripcion.CompeticionesInscritas.Select(ci => new CompeticionInscritaDetalleDTO
                {
                    CompeticionId = ci.CompeticionId,
                    Nombre = ci.Competicion?.Nombre ?? string.Empty,
                    TipoDeporte = ci.Competicion?.TipoDeporte?.Nombre ?? string.Empty,
                    Fecha = ci.Competicion?.Fecha ?? default,
                    Precio = ci.Competicion?.Precio ?? 0,
                    ProblemasFisicos = ci.ProblemasFisicos
                }).ToList()
            };

            return Ok(dto);
        }

       
        /// POST: api/InscripcionesCompeticiones
        
        [HttpPost]
        [ProducesResponseType(typeof(InscripcionCompeticionDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<InscripcionCompeticionDetalleDTO>> CrearInscripcion(
            [FromBody] CrearInscripcionCompeticionDTO dto,
            CancellationToken cancellationToken)
        {
            if (dto.CompeticionesSeleccionadas == null || !dto.CompeticionesSeleccionadas.Any())
            {
                return BadRequest("No se ha seleccionado ninguna competición para inscribirse.");
            }

            var competicionesIds = dto.CompeticionesSeleccionadas.Select(c => c.CompeticionId).Distinct().ToList();
            if (competicionesIds.Count != dto.CompeticionesSeleccionadas.Count)
            {
                return BadRequest("No se puede inscribir en la misma competición más de una vez.");
            }

            var competicionesBD = await _context.Competiciones
                .Include(c => c.TipoDeporte)
                .Include(c => c.CompeticionesInscritas)
                    .ThenInclude(ci => ci.Inscripcion)
                .Where(c => competicionesIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (competicionesBD.Count != competicionesIds.Count)
            {
                return BadRequest("Alguna de las competiciones seleccionadas no existe en la base de datos.");
            }

            var sinPlazas = competicionesBD.Where(c => c.CompeticionesInscritas.Count >= c.Plazas).ToList();
            if (sinPlazas.Any())
            {
                return BadRequest($"No quedan plazas libres en: {string.Join(", ", sinPlazas.Select(c => c.Nombre))}.");
            }

            var usuario = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.NormalizedEmail == dto.Email.ToUpperInvariant(),
                    cancellationToken);

            if (usuario == null)
            {
                return BadRequest("El usuario no está registrado en el sistema.");
            }

            var yaInscritas = competicionesBD
                .Where(c => c.CompeticionesInscritas.Any(ci => ci.Inscripcion != null && ci.Inscripcion.UserId == usuario.Id))
                .ToList();
            if (yaInscritas.Any())
            {
                return BadRequest($"Ya estás inscrito en: {string.Join(", ", yaInscritas.Select(c => c.Nombre))}.");
            }

            if (!Enum.TryParse<MetodoPago>(dto.MetodoPago, true, out var metodoPagoEnum))
            {
                return BadRequest($"Método de pago '{dto.MetodoPago}' no válido.");
            }

            var precioTotal = competicionesBD.Sum(c => c.Precio);

            var nuevaInscripcion = new Inscripcion
            {
                FechaInscripcion = DateTime.Now,
                PrecioTotal = precioTotal,
                MetodoPago = metodoPagoEnum,
                UserId = usuario.Id,
                ApplicationUser = usuario,
                CompeticionesInscritas = new List<CompeticionInscrita>(),
                ClasesInscritas = new List<ClaseInscrita>()
            };

            foreach (var item in dto.CompeticionesSeleccionadas)
            {
                nuevaInscripcion.CompeticionesInscritas.Add(new CompeticionInscrita
                {
                    CompeticionId = item.CompeticionId,
                    Inscripcion = nuevaInscripcion,
                    ProblemasFisicos = item.ProblemasFisicos ?? string.Empty
                });
            }

            try
            {
                await _context.Inscripciones.AddAsync(nuevaInscripcion, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la inscripción a competiciones en la BD.");
                return StatusCode(500, "Error interno del servidor al procesar la inscripción.");
            }

            var respuestaDTO = new InscripcionCompeticionDetalleDTO
            {
                IdInscripcion = nuevaInscripcion.Id,
                FechaInscripcion = nuevaInscripcion.FechaInscripcion,
                PrecioTotal = nuevaInscripcion.PrecioTotal,
                MetodoPago = nuevaInscripcion.MetodoPago.ToString(),
                NombreCliente = usuario.Name ?? string.Empty,
                ApellidosCliente = usuario.Surname ?? string.Empty,
                Email = usuario.Email ?? string.Empty,
                Telefono = usuario.PhoneNumber ?? string.Empty,
                CompeticionesInscritas = nuevaInscripcion.CompeticionesInscritas.Select(ci =>
                {
                    var competicion = competicionesBD.First(c => c.Id == ci.CompeticionId);
                    return new CompeticionInscritaDetalleDTO
                    {
                        CompeticionId = competicion.Id,
                        Nombre = competicion.Nombre,
                        TipoDeporte = competicion.TipoDeporte?.Nombre ?? string.Empty,
                        Fecha = competicion.Fecha,
                        Precio = competicion.Precio,
                        ProblemasFisicos = ci.ProblemasFisicos
                    };
                }).ToList()
            };

            return CreatedAtAction(nameof(GetInscripcionById), new { idInscripcion = nuevaInscripcion.Id }, respuestaDTO);
        }
    }
}
