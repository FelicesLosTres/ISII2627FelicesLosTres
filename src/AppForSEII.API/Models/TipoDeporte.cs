using System.ComponentModel.DataAnnotatons;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
[Index(nameof(Nombre), IsUnique = true)]
public class TipoDeporte
{
[Key]
public int Id { get; set; }

[Required(ErrorMessage = "El nombre del deporte es obligatorio.")]
[StringLength(50)]
public string Nombre { get; set; } = string.Empty;

// Relación 1:N con Pista
public IList<Pista> Pistas { get; set; }

// Relación 1:N con Material
public IList<Material> Materiales { get; set; }

// Relación 1:N con Competicion
public IList<Competicion> Competiciones { get; set; }

// Relación 1:N con ClaseDeportiva
//public IList<ClaseDeportiva> ClasesDeportivas { get; set; }

public TipoDeporte()
{

}

public TipoDeporte(string nombre)
{
Nombre = nombre;
}
}
}