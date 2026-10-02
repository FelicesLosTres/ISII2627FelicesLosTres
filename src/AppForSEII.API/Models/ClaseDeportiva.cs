using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
public class ClaseDeportiva
{
[Key]
public int IdClaseDeportiva { get; set; }

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

//[ForeignKey(nameof(IdTipoDeporte))]
public TipoDeporte? TipoDeporte { get; set; }
// Constructor vacío
public ClaseDeportiva()
{
}
// Constructor simple
public ClaseDeportiva(
string nombre,
string descripcion,
int duracion,
int idTipoDeporte)
{
Nombre = nombre;
Descripcion = descripcion;
Duracion = duracion;
IdTipoDeporte = idTipoDeporte;
}
}
}