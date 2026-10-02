using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
[PrimaryKey(nameof(IdPista), nameof(IdReserva))]
public class PistaReservada
{

[Required(ErrorMessage = "La cantidad es obligatoria.")]
[Range(1, 100, ErrorMessage = "La cantidad debe ser mayor que 0.")]
public int Cantidad { get; set; }

[Required(ErrorMessage = "El precio es obligatorio.")]
[Range(0.01, 99999, ErrorMessage = "El precio debe ser mayor que 0.")]
public decimal Precio { get; set; }

[Required]
public int IdPista { get; set; }

[ForeignKey(nameof(IdPista))]
public Pista? Pista { get; set; }

[Required]
public int IdReserva { get; set; }

[ForeignKey(nameof(IdReserva))]
public Reserva? Reserva { get; set; }


[StringLength(250,
ErrorMessage = "Las observaciones no pueden superar los 250 caracteres.")]
public string Observaciones { get; set; }

// Constructor vacío
public PistaReservada()
{
}

// Constructor simple
public PistaReservada( int cantidad, decimal precio, int idPista, int idReserva)
{
Cantidad = cantidad;
Precio = precio;
IdPista = idPista;
IdReserva = idReserva;

}
}
}