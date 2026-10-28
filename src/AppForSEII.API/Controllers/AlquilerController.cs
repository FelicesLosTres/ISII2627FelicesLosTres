  using AppForSEII.API.DTOs.AlquilerDTOs;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquilerController> _logger;

        public AlquilerController(ApplicationDbContext context, ILogger<AlquilerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAlquiler(int id)
        {

            var Alquiler = await _context.Alquileres
             .Where(a => a.IdAlquiler == id)
                 .Include(a => a.MaterialesAlquilados) //join table MaterialAlquilado
                    .ThenInclude(ri => ri.Material) //then join table Material
                        .ThenInclude(Material => Material.TipoMaterial) //then join table TipoMaterial
                .Include(a => a.MaterialesAlquilados)
                    .ThenInclude(ri => ri.Material)
                        .ThenInclude(Material => Material. TipoDeporte)
             .Select(a => new AlquilerDetailDTO(a.IdAlquiler,
                    a.PrecioTotal, a.FechaAlquiler,
                    a.ApplicationUser.DNI,
                    a.ApplicationUser.Name, a.ApplicationUser.Surname,
                    a.ApplicationUser.Email, a.ApplicationUser.TelefonoUsuario, a.MetodoPago, a.MaterialesAlquilados
                        .Select(ri => new AlquilerItemDTO(ri.Material.IdMaterial,
                                ri.Material.Nombre, ri.Material.TipoMaterial.NombreTipoMaterial,
                                ri.Material.TipoDeporte.Nombre)).ToList()))
             .FirstOrDefaultAsync();


            if (Alquiler == null)
            {
                _logger.LogError($"Error: El alquiler con id {id} no existe");
                return NotFound();
            }


            return Ok(Alquiler);
        }
    }

}