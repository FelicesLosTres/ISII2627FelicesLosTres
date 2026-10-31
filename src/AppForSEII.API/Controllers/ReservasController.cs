  using AppForSEII.API.DTOs.ReservaDTOs;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;

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

        /// Obtiene el detalle de una reserva realizada (Paso 7)
        /// GET: api/reservas/5
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReserva(int idReserva)
        { 
            var reserva = await _context.Reservas
               .Where(r => r.IdReserva == idReserva)
                   .Include(r => r.PistasReservadas) 
                      .ThenInclude(rp => rp.Pista)
                          .ThenInclude(pista => pista.TipoDeporte)
               .Select(r => new ReservaDetalleDTO(r.IdReserva, r.FechaReserva, r.ApplicationUser.Name, r.ApplicationUser.Surname,
                      r.ApplicationUser.Email, r.ApplicationUser.PhoneNumber, r.PrecioTotal, r.MetodoPago, 
                      r.PistasReservadas
                      .Select(pr => new ReservaItemDTO(
                          pr.Pista.NombrePista,
                          pr.Pista.TipoDeporte.Nombre,
                          pr.Precio,
                          pr.Pista.NPersonas,
                          pr.Observaciones)).ToList<ReservaItemDTO>())).FirstOrDefaultAsync();
            if (reserva == null)
            {
                _logger.LogWarning("Reserva con IdReserva={IdReserva} no encontrada.", idReserva);
                return NotFound();
            }

            return Ok(reserva);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CrearReserva(CrearReservaDTO dto)
        {
            if (dto.PistasSeleccionadas.Count == 0)
            {
                ModelState.AddModelError("PistasSeleccionadas", "Error! Debes seleccionar almenos una pista para ser reservada.");
            }

            var usuario = _context.ApplicationUsers.FirstOrDefault(au => au.Email == dto.CorreoCliente);
            if (usuario == null)
            ModelState.AddModelError("ReservaApplicationuser", "Error! El usuario no está registrado");

            if (ModelState.ErrorCount > 0)
            return BadRequest(new ValidationProblemDetails(ModelState));

            var PistasNombres = dto.PistasSeleccionadas.Select(ps => ps.NombrePista).ToList<string>();
            var pistas = _context.Pistas.Include(p => p.PistasReservadas)
            .ThenInclude(ps => ps.Reserva)
            .Where(p => PistasNombres.Contains(p.NombrePista))
            .ToList();

            Reserva reserva = new Reserva(DateTime.Now, (MetodoPago)dto.metodo, usuario, new List<PistaReservada>());
            foreach(var item in dto.PistasSeleccionadas)
            {
                var pista = pistas.FirstOrDefault(p => p.NombrePista == item.NombrePista);
                if ((pista == null))
                {
                    ModelState.AddModelError("PistasReservadas", $"Error! La pista llamada '{item.NombrePista}' no está disponible para ser reservada");

                }
                else
                {
                    reserva.PistasReservadas.Add(new PistaReservada(1, pista.Precio, pista.IdPista, reserva.IdReserva));
                    item.Precio = pista.Precio;
                }
            }
            reserva.PrecioTotal = reserva.PistasReservadas.Sum(pr => pr.Precio);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(reserva);

            try {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("reserva", $"Error! Ha habido un error a la hora de hacer la reserva");
                return Conflict("Error" + ex.Message);

            }
            var reservaDetail = new ReservaDetalleDTO(reserva.IdReserva, reserva.FechaReserva, usuario.Name, usuario.Surname, usuario.Email, usuario.PhoneNumber , reserva.PrecioTotal, (MetodoPago)dto.metodo, (IList<ReservaItemDTO>)dto.PistasSeleccionadas);
            return CreatedAtAction("GetReserva", new { id = reserva.IdReserva }, reservaDetail);

        }

    }
}