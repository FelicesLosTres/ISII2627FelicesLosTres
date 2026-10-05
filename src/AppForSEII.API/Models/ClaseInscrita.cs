using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
    [PrimaryKey(nameof(ClaseDeportivaId), nameof(InscripcionId))]
public class ClaseInscrita
{

[Required]
public int ClaseDeportivaId { get; set;}

[ForeignKey(nameof(ClaseDeportivaId))]
public ClaseDeportiva? ClaseDeportiva { get; set;}

[Required]
public int InscripcionId { get; set;}

[ForeignKey(nameof(InscripcionId))]
public Inscripcion? Inscripcion { get; set;}

[StringLength(250,
ErrorMessage = "Las observaciones no pueden superar los 250 caracteres.")]
public string Observaciones { get; set;}

[Required(ErrorMessage = "Las plazas reservadas son obligatorias.")]
[Range(1, 100,
ErrorMessage = "Las plazas reservadas deben ser mayores que 0.")]
public int PlazasReservadas { get; set;}

[Required(ErrorMessage = "El precio es obligatorio.")]
[Range(0.01, 99999,
ErrorMessage = "El precio debe ser mayor que 0.")]
[Precision(18, 2)]
public decimal Precio { get; set; }

// Constructor vacío
public ClaseInscrita()
{
}

// Constructor simple
public ClaseInscrita(
int claseDeportivaId,
int inscripcionId,
string observaciones,
int plazasReservadas,
decimal precio)
{
ClaseDeportivaId = claseDeportivaId;
InscripcionId = inscripcionId;
Observaciones = observaciones;
PlazasReservadas = plazasReservadas;
Precio = precio;
}
}
}