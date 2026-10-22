using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.MaterialDTOs;
using AppForSEII.API.Data;
using AppForSEII.API.Models;
using System.Net;


namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaterialesController> _logger;

        public MaterialesController(ApplicationDbContext context, ILogger<MaterialesController> logger)
        {
            _context = context;
            _logger = logger;
        }


        //Metodo Get que devuelve los materiales disponibles para alquilar pudiendo filtrarlos por precio máximo y tipo de material.
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<MaterialDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetMaterialesReservar(String? tipoMaterial, decimal? precioMax)
        {
            
            if (precioMax != null && precioMax < 0) //Se evitan los números negativos
            {
                ModelState.AddModelError("precioMax", "El precio máximo seleccionado no puede ser menor que 0");
                _logger.LogError($"{DateTime.Now} Error: El precio máximo seleccionado no puede ser menor que 0");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            IList<MaterialDTO> materiales = await _context.Materiales

                //join table Material and table TipoMaterial
                .Include(m => m.TipoMaterial)

                //join tables Material and MaterialAlquilado and then Alquiler
                .Include(m => m.MaterialesAlquilados).ThenInclude(ri => ri.Alquiler)

                .Where(m => // where clause
                   (tipoMaterial == null || m.TipoMaterial.NombreTipoMaterial.Contains(tipoMaterial)) //in case user has provided a TipoMaterial
                    && (precioMax == null || m.Precio <= precioMax) //Selecciona los articulos con precio menor o todos si no se establece precio.
                    )

                //.OrderBy(m => m.Title)

                .Select(m => new MaterialDTO(m.IdMaterial, m.Nombre, m.TipoMaterial.NombreTipoMaterial, m.TipoDeporte.Nombre, m.Precio, m.Cantidad))
                .ToListAsync();

            return Ok(materiales);
        }

    }
}