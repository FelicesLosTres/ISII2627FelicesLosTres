
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{

public class Reserva
{
    
[Key]
public int IdReserva { get; set; }

[Required(ErrorMessage = "La fecha de la reserva es obligatoria.")]
public DateTime FechaReserva { get; set; }

[Required(ErrorMessage = "El precio total es obligatorio.")]
[Range(0.01, 99999,
ErrorMessage = "El precio total debe ser mayor que 0.")]
public decimal PrecioTotal { get; set; }

[Required(ErrorMessage = "El método de pago es obligatorio.")]
[StringLength(30,
ErrorMessage = "El método de pago no puede superar los 30 caracteres.")]
public string MetodoPago { get; set; } = string.Empty;



// Constructor vacío
public Reserva()
{
}

// Constructor simple
public Reserva(
DateTime fechaReserva,
decimal precioTotal,
string metodoPago)
{
FechaReserva = fechaReserva;
PrecioTotal = precioTotal;
MetodoPago = metodoPago;

}
}
}