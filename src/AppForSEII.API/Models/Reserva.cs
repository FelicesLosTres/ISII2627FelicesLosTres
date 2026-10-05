
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
[Precision(18, 2)]
public decimal PrecioTotal { get; set; }

[Required(ErrorMessage = "El método de pago es obligatorio.")]
[StringLength(30,
ErrorMessage = "El método de pago no puede superar los 30 caracteres.")]
public MetodoPago MetodoPago { get; set; }

// Relación 1:N con PistaReservada
public IList<PistaReservada> PistasReservadas { get; set; }

// FK con ApplicationUser
[Required]
public string UserId { get; set; } 

[ForeignKey(nameof(UserId))]
public ApplicationUser ApplicationUser { get; set; }

// Constructor vacío
public Reserva()
{
}

// Constructor simple
public Reserva(
DateTime fechaReserva,
decimal precioTotal,
MetodoPago metodoPago, ApplicationUser applicationUser)
{
FechaReserva = fechaReserva;
PrecioTotal = precioTotal;
MetodoPago = metodoPago;
ApplicationUser = applicationUser;
}
}
}