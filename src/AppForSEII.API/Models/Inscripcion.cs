using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
public class Inscripcion
{
[Key]
public int Id { get; set; }

[Required(ErrorMessage = "La fecha de inscripción es obligatoria.")]
public DateTime FechaInscripcion { get; set; }

[Required(ErrorMessage = "El método de pago es obligatorio.")]
public MetodoPago MetodoPago { get; set; }

[Required(ErrorMessage = "El precio total es obligatorio.")]
[Range(0.01, 99999,
ErrorMessage = "El precio total debe ser mayor que 0.")]
public decimal PrecioTotal { get; set; }

// Relación N:1 con ApplicationUser
//[Required]
//public string ClienteId { get; set; } 

// Relación 1:N con ClaseInscrita
//public IList<ClaseInscrita> ClasesInscritas { get; set; }

// Constructor vacío
public Inscripcion()
{
    
}

// Constructor simple
public Inscripcion(
DateTime fechaInscripcion,
MetodoPago metodoPago,
decimal precioTotal,
string clienteId)
{
FechaInscripcion = fechaInscripcion;
MetodoPago = metodoPago;
PrecioTotal = precioTotal;
ClienteId = clienteId;
}
}
}