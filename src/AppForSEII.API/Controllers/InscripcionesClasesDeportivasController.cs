using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ClaseDeportivaDTOs;
using AppForSEII.API.DTOs.InscripcionClaseDeportivaDTOs;
using AppForSEII.API.Models;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionesClasesDeportivasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InscripcionesClasesDeportivasController> _logger;

        public InscripcionesClasesDeportivasController(
            ApplicationDbContext context,
            ILogger<InscripcionesClasesDeportivasController> logger)
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
        [ProducesResponseType(typeof(IList<ClaseDeportivaDTO>), (int)HttpStatusCode.OK)]
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

            IList<ClaseDeportivaDTO> clases = await _context.ClasesDeportivas
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
                .Select(c => new ClaseDeportivaDTO(
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

        /// <summary>
        /// Crea una inscripción a una o varias clases deportivas.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(InscripcionClaseDeportivaDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<InscripcionClaseDeportivaDetailDTO>> CreateInscripcion(
            [FromBody] InscripcionClaseDeportivaCreateDTO inscripcionForCreate,
            CancellationToken cancellationToken)
        {
            var idsClases = inscripcionForCreate.ClasesInscritas
                .Select(c => c.ClaseDeportivaId);

            if (idsClases.Distinct().Count() != idsClases.Count())
            {
                ModelState.AddModelError(
                    nameof(inscripcionForCreate.ClasesInscritas),
                    "No se puede incluir la misma clase más de una vez.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (!Enum.TryParse<MetodoPago>(inscripcionForCreate.MetodoPago, true, out var metodoPago) ||
                !Enum.IsDefined(typeof(MetodoPago), metodoPago))
            {
                ModelState.AddModelError(
                    nameof(inscripcionForCreate.MetodoPago),
                    $"El método de pago '{inscripcionForCreate.MetodoPago}' no es válido.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var cliente = await _context.Users.FirstOrDefaultAsync(
                u => u.NormalizedEmail == inscripcionForCreate.Email.ToUpperInvariant(),
                cancellationToken);
            if (cliente == null)
            {
                ModelState.AddModelError(
                    nameof(inscripcionForCreate.Email),
                    $"No existe ningún cliente registrado con el correo {inscripcionForCreate.Email}.");
                return NotFound(new ValidationProblemDetails(ModelState));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.Serializable,
                cancellationToken);

            var clases = await _context.ClasesDeportivas
                .Include(c => c.TipoDeporte)
                .Where(c => idsClases.Contains(c.IdClaseDeportiva))
                .ToListAsync(cancellationToken);

            if (clases.Count != idsClases.Count())
            {
                ModelState.AddModelError(
                    nameof(inscripcionForCreate.ClasesInscritas),
                    "Alguna de las clases seleccionadas no existe.");
                return NotFound(new ValidationProblemDetails(ModelState));
            }

            var clasesYaInscritas = await _context.ClasesInscritas
                .Where(ci => idsClases.Contains(ci.ClaseDeportivaId) &&
                             ci.Inscripcion != null &&
                             ci.Inscripcion.UserId == cliente.Id)
                .Select(ci => ci.ClaseDeportivaId)
                .ToListAsync(cancellationToken);
            if (clasesYaInscritas.Count > 0)
            {
                var nombresClases = clases
                    .Where(c => clasesYaInscritas.Contains(c.IdClaseDeportiva))
                    .Select(c => c.Nombre);
                ModelState.AddModelError(
                    nameof(inscripcionForCreate.ClasesInscritas),
                    $"El cliente ya está inscrito en: {string.Join(", ", nombresClases)}.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            foreach (var item in inscripcionForCreate.ClasesInscritas)
            {
                var clase = clases.First(c => c.IdClaseDeportiva == item.ClaseDeportivaId);
                if (clase.PlazasDisponibles < item.PlazasReservadas)
                {
                    ModelState.AddModelError(
                        nameof(item.PlazasReservadas),
                        $"No hay suficientes plazas para la clase '{clase.Nombre}'. " +
                        $"Disponibles: {clase.PlazasDisponibles}, solicitadas: {item.PlazasReservadas}.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
            }

            var inscripcion = new Inscripcion
            {
                FechaInscripcion = DateTime.Now,
                MetodoPago = metodoPago,
                UserId = cliente.Id,
                ApplicationUser = cliente,
                ClasesInscritas = new List<ClaseInscrita>(),
                CompeticionesInscritas = new List<CompeticionInscrita>()
            };

            foreach (var item in inscripcionForCreate.ClasesInscritas)
            {
                var clase = clases.First(c => c.IdClaseDeportiva == item.ClaseDeportivaId);
                clase.PlazasDisponibles -= item.PlazasReservadas;

                var precio = clase.PrecioUnitario * item.PlazasReservadas;
                inscripcion.PrecioTotal += precio;
                inscripcion.ClasesInscritas.Add(new ClaseInscrita
                {
                    ClaseDeportivaId = clase.IdClaseDeportiva,
                    ClaseDeportiva = clase,
                    PlazasReservadas = item.PlazasReservadas,
                    Precio = precio,
                    Observaciones = item.Observaciones ?? string.Empty
                });
            }

            try
            {
                _context.Inscripciones.Add(inscripcion);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la inscripción a clases deportivas.");
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    "Error interno del servidor al procesar la inscripción.");
            }

            return StatusCode((int)HttpStatusCode.Created, ToDetailDTO(inscripcion));
        }

        private static InscripcionClaseDeportivaDetailDTO ToDetailDTO(Inscripcion inscripcion)
        {
            return new InscripcionClaseDeportivaDetailDTO
            {
                IdInscripcion = inscripcion.Id,
                FechaInscripcion = inscripcion.FechaInscripcion,
                PrecioTotal = inscripcion.PrecioTotal,
                MetodoPago = inscripcion.MetodoPago.ToString(),
                NombreCliente = inscripcion.ApplicationUser?.Name ?? string.Empty,
                ApellidosCliente = inscripcion.ApplicationUser?.Surname ?? string.Empty,
                Email = inscripcion.ApplicationUser?.Email ?? string.Empty,
                Telefono = inscripcion.ApplicationUser?.PhoneNumber ?? string.Empty,
                ClasesInscritas = new List<InscripcionClaseDeportivaItemDTO>(inscripcion.ClasesInscritas.Select(ci =>
                    new InscripcionClaseDeportivaItemDTO
                    {
                        ClaseDeportivaId = ci.ClaseDeportivaId,
                        Nombre = ci.ClaseDeportiva?.Nombre ?? string.Empty,
                        TipoDeporte = ci.ClaseDeportiva?.TipoDeporte?.Nombre ?? string.Empty,
                        FechaHora = ci.ClaseDeportiva?.FechaHora ?? default,
                        PlazasReservadas = ci.PlazasReservadas,
                        Precio = ci.Precio,
                        Observaciones = ci.Observaciones
                    }))
            };
        }
    }
}