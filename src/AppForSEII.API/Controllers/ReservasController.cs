using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.Data;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.ReservaDTOs;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservasController> _logger;

        public ReservasController(ApplicationDbContext context, ILogger<ReservasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el detalle de una reserva realizada por su IdReserva (Paso 7)
        /// GET: api/reservas/5
        /// </summary>
        [HttpGet("{idReserva:int}")]
        [ProducesResponseType(typeof(ReservaDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReservaDetalleDTO>> GetReservaById(
            int idReserva,
            CancellationToken cancellationToken)
        {
            var reserva = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.ApplicationUser)
                .Include(r => r.PistasReservadas)
                    .ThenInclude(pr => pr.Pista)
                        .ThenInclude(p => p!.TipoDeporte)
                .FirstOrDefaultAsync(r => r.IdReserva == idReserva, cancellationToken);

            if (reserva == null)
            {
                _logger.LogWarning("Reserva con IdReserva={IdReserva} no encontrada.", idReserva);
                return NotFound($"No existe ninguna reserva con la clave {idReserva}.");
            }

            var dto = new ReservaDetalleDTO
            {
                IdReserva = reserva.IdReserva,
                FechaReserva = reserva.FechaReserva,
                PrecioTotal = reserva.PrecioTotal,
                MetodoPago = reserva.MetodoPago.ToString(),
                NombreCliente = reserva.ApplicationUser?.Name ?? string.Empty,
                ApellidosCliente = reserva.ApplicationUser?.Surname ?? string.Empty,
                Email = reserva.ApplicationUser?.Email ?? string.Empty,
                Telefono = reserva.ApplicationUser?.PhoneNumber ?? string.Empty,
                PistasReservadas = reserva.PistasReservadas.Select(pr => new PistaReservadaDetalleDTO
                {
                    PistaId = pr.IdPista,
                    NombrePista = pr.Pista?.NombrePista ?? string.Empty,
                    NombreDeporte = pr.Pista?.TipoDeporte?.Nombre ?? string.Empty,
                    Aforo = pr.Pista?.NPersonas ?? 0,
                    Precio = pr.Precio,
                    Observaciones = pr.Observaciones
                }).ToList()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Finaliza y guarda una reserva de pistas (Pasos 5 y 6)
        /// POST: api/reservas
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ReservaDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ReservaDetalleDTO>> FinalizarReserva(
            [FromBody] CrearReservaDTO dto,
            CancellationToken cancellationToken)
        {
            if (dto.PistasSeleccionadas == null || !dto.PistasSeleccionadas.Any())
            {
                return BadRequest("No se ha seleccionado ninguna pista para reservar.");
            }

            var pistasIds = dto.PistasSeleccionadas.Select(p => p.PistaId).Distinct().ToList();
            if (pistasIds.Count != dto.PistasSeleccionadas.Count)
            {
                return BadRequest("No se puede reservar la misma pista más de una vez.");
            }

            var pistasBD = await _context.Pistas
                .AsNoTracking()
                .Include(p => p.TipoDeporte)
                .Where(p => pistasIds.Contains(p.IdPista))
                .ToListAsync(cancellationToken);

            if (pistasBD.Count != pistasIds.Count)
            {
                return BadRequest("Alguna de las pistas seleccionadas no existe en la base de datos.");
            }

            var usuario = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.NormalizedEmail == dto.Email.ToUpperInvariant(),
                    cancellationToken);

            if (usuario == null)
            {
                return BadRequest("El usuario no está registrado en el sistema.");
            }

            var precioTotal = pistasBD.Sum(p => p.Precio);

            if (!Enum.TryParse<MetodoPago>(dto.MetodoPago, true, out var metodoPagoEnum))
            {
                return BadRequest($"Método de pago '{dto.MetodoPago}' no válido.");
            }

            var nuevaReserva = new Reserva
            {
                FechaReserva = DateTime.Now,
                PrecioTotal = precioTotal,
                MetodoPago = metodoPagoEnum,
                UserId = usuario.Id,
                ApplicationUser = usuario,
                PistasReservadas = new List<PistaReservada>()
            };

            foreach (var item in dto.PistasSeleccionadas)
            {
                var pistaBD = pistasBD.First(p => p.IdPista == item.PistaId);

                nuevaReserva.PistasReservadas.Add(new PistaReservada
                {
                    IdPista = pistaBD.IdPista,
                    Precio = pistaBD.Precio,
                    Cantidad = 1,
                    Observaciones = item.Observaciones ?? string.Empty
                });
            }

            try
            {
                await _context.Reservas.AddAsync(nuevaReserva, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la reserva en la BD.");
                return StatusCode(500, "Error interno del servidor al procesar la reserva.");
            }

            var respuestaDTO = new ReservaDetalleDTO
            {
                IdReserva = nuevaReserva.IdReserva,
                FechaReserva = nuevaReserva.FechaReserva,
                PrecioTotal = nuevaReserva.PrecioTotal,
                MetodoPago = nuevaReserva.MetodoPago.ToString(),
                NombreCliente = usuario.Name ?? string.Empty,
                ApellidosCliente = usuario.Surname ?? string.Empty,
                Email = usuario.Email ?? string.Empty,
                Telefono = usuario.PhoneNumber ?? string.Empty,
                PistasReservadas = nuevaReserva.PistasReservadas.Select(pr => new PistaReservadaDetalleDTO
                {
                    PistaId = pr.IdPista,
                    NombrePista = pistasBD.First(p => p.IdPista == pr.IdPista).NombrePista,
                    NombreDeporte = pistasBD.First(p => p.IdPista == pr.IdPista).TipoDeporte?.Nombre ?? string.Empty,
                    Aforo = pistasBD.First(p => p.IdPista == pr.IdPista).NPersonas,
                    Precio = pr.Precio,
                    Observaciones = pr.Observaciones
                }).ToList()
            };

            return CreatedAtAction(nameof(GetReservaById), new { idReserva = nuevaReserva.IdReserva }, respuestaDTO);
        }
    }
}