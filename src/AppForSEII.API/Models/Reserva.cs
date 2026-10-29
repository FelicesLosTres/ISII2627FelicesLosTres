
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{

public class Reserva
{
    
[Key]
public int IdReserva { get; set; }

[Required(ErrorMessage = "La fecha de la reserva es obligatoria.")]
public DateTime FechaReserva { get; set; }= DateTime.Today;

[Required(ErrorMessage = "El precio total es obligatorio.")]
[Range(0.01, 99999,
ErrorMessage = "El precio total debe ser mayor que 0.")]
[Precision(18, 2)]
public double PrecioTotal { get; set; }=10;

[Required(ErrorMessage = "El método de pago es obligatorio.")]
[StringLength(30,
ErrorMessage = "El método de pago no puede superar los 30 caracteres.")]
public MetodoPago MetodoPago { get; set; }= MetodoPago.Efectivo;

// Relación 1:N con PistaReservada
public IList<PistaReservada> PistasReservadas { get; set; } = new List<PistaReservada>();

// FK con ApplicationUser
[Required]
public string UserId { get; set; } = string.Empty;

[ForeignKey(nameof(UserId))]
public ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();

// Constructor vacío
public Reserva()
{
}

// Constructor simple
public Reserva(
DateTime fechaReserva,
MetodoPago metodoPago, ApplicationUser applicationUser)
{
FechaReserva = fechaReserva;
PrecioTotal = PistasReservadas.Sum(pr => pr.Precio);
MetodoPago = metodoPago;
ApplicationUser = applicationUser;
UserId = applicationUser.Id;
}
}
}