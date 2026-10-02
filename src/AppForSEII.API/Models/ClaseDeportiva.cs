using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
public class ClaseDeportiva
{
[Key]
public int IdClaseDeportiva { get; set; }

[Required(ErrorMessage = "La fecha y hora son obligatorias.")]
public DateTime FechaHora { get; set; }

[Required(ErrorMessage = "El lugar es obligatorio.")]
[StringLength(100)]
public string Lugar { get; set; } 

[Required(ErrorMessage = "El monitor es obligatorio.")]
[StringLength(100)]
public string Monitor { get; set; } 

[Required(ErrorMessage = "El nivel es obligatorio.")]
[StringLength(50)]
public string Nivel { get; set; }

[Required(ErrorMessage = "Las plazas disponibles son obligatorias.")]
[Range(1, 1000)]
public int PlazasDisponibles { get; set; }

[Required(ErrorMessage = "El precio unitario es obligatorio.")]
[Range(0.01, 99999)]
public decimal PrecioUnitario { get; set; }

[Required(ErrorMessage = "El nombre de la clase es obligatorio.")]
[StringLength(50, MinimumLength = 3,
ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
public string Nombre { get; set; } 

[Required(ErrorMessage = "La descripción es obligatoria.")]
[StringLength(500,
ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
public string Descripcion { get; set; } 


[Required(ErrorMessage = "La duración es obligatoria.")]
[Range(1, 300,
ErrorMessage = "La duración debe estar entre 1 y 300 minutos.")]
public int Duracion { get; set; }

// Relación N:1 con TipoDeporte
[Required]
public int IdTipoDeporte { get; set; }

//relacion a claseinscrita
public IList<ClaseInscrita> ClasesInscritas { get; set; }

[ForeignKey(nameof(IdTipoDeporte))]
public TipoDeporte? TipoDeporte { get; set; }
// Constructor vacío
public ClaseDeportiva()
{
}
// Constructor simple
public ClaseDeportiva(string nombre, string descripcion, int duracion, int idTipoDeporte, DateTime fechaHora, string lugar, string monitor, string nivel, int plazasDisponibles, decimal precioUnitario)
{
Nombre = nombre;
Descripcion = descripcion;
Duracion = duracion;
IdTipoDeporte = idTipoDeporte;
FechaHora = fechaHora;
Lugar = lugar;
Monitor = monitor;
Nivel = nivel;
PlazasDisponibles = plazasDisponibles;
PrecioUnitario = precioUnitario;
}
}
}