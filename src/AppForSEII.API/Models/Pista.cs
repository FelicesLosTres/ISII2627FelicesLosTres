using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
    [Index(nameof(NombrePista), IsUnique = true)]

public class Pista
{
[Key]
public int IdPista { get; set; }

[Required(ErrorMessage = "El nombre de la pista es obligatorio.")]
[StringLength(50, Minimumegth = 3, ErrorMessage = "El nombre debe teer etre 3 y 50 caracteres.")]
public string NombrePista { get; set; }

[Range(1, 20, ErrorMessage = "El número de personas debe estar entre 1 y 20.")]
public int NPersonas { get; set; }

[Column(TypeName = "decimal(10,2)")]
[Range(0.01, 1000.00, ErrorMessage = "El precio debe estar entre 0.01 y 1000.00.")]
public decimal Precio { get; set; }

[Range(0, 100, ErrorMessage = "El stock no puede ser negativo.")]
public int Stock { get; set; }

// Clave foránea hacia TipoDeporte
[Required]
public int IdTipoDeporte { get; set; }

[ForeignKey(nameof(IdTipoDeporte))]
public TipoDeporte? TipoDeporte { get; set; }

// Relación 1:N con PistaReservada
public IListn<PistaReservada> PistasReservadas { get; set; }

//Constructor vacío
public Pista()
{
}

//Costructor simple
public Pista(string nombrePista, int nPersonas, decimal precio, int stock, int idTipoDeporte)
{
    NombrePista = nombrePista;
    NPersonas = nPersonas;
    Precio = precio;
    Stock = stock;
    IdTipoDeporte = idTipoDeporte;
    PistasReservadas = new List<PistaReservada>();
}


}
}