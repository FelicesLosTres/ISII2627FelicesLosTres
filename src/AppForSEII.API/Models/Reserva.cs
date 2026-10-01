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

[Required(ErrorMessage = "Los datos de pago son obligatorios.")]
[StringLength(100,
ErrorMessage = "Los datos de pago no pueden superar los 100 caracteres.")]
public string DatosPago { get; set; } = string.Empty;

// FK con ApplicationUser
[Required]
public string UserId { get; set; } = string.Empty;

[ForeignKey(nameof(UserId))]
public ApplicationUser? Usuario { get; set; }

// Relación 1:N con PistaReservada
public IList<PistaReservada> PistasReservadas { get; set; }

// Constructor vacío
public Reserva()
{
PistasReservadas = new List<PistaReservada>();
}

// Constructor simple
public Reserva(
DateTime fechaReserva,
decimal precioTotal,
string metodoPago,
string datosPago,
string userId)
{
FechaReserva = fechaReserva;
PrecioTotal = precioTotal;
MetodoPago = metodoPago;
DatosPago = datosPago;
UserId = userId;

PistasReservadas = new List<PistaReservada>();
}
}
}