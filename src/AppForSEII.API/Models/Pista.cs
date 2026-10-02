using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
[Index(nameof(NombrePista), IsUnique = true)]
public class Pista
{
[Key]
public int IdPista { get; set; }

[Required(ErrorMessage = "El nombre de la pista es obligatorio.")]
[StringLength(50, MinimumLength = 3,
ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
public string NombrePista { get; set; }

[Required(ErrorMessage = "El número de personas es obligatorio.")]
[Range(1, 100,
ErrorMessage = "El número de personas debe estar entre 1 y 100.")]
public int NPersonas { get; set; }

[Required(ErrorMessage = "El precio es obligatorio.")]
[Range(0.01, 99999,
ErrorMessage = "El precio debe ser mayor que 0.")]
public decimal Precio { get; set; }

[Required(ErrorMessage = "El stock es obligatorio.")]
[Range(0, 1000,
ErrorMessage = "El stock no puede ser negativo.")]
public int Stock { get; set; }

// Clave foránea hacia TipoDeporte
[Required]
public int IdTipoDeporte { get; set; }

[ForeignKey(nameof(IdTipoDeporte))]
public TipoDeporte? TipoDeporte { get; set; }

// Relación 1:N con PistaReservada
public IList<PistaReservada> PistasReservadas { get; set; }

// Constructor vacío
public Pista()
{
}

// Constructor simple
public Pista(
string nombrePista,
int nPersonas,
decimal precio,
int stock
)
{
NombrePista = nombrePista;
NPersonas = nPersonas;
Precio = precio;
Stock = stock;

}
}
}