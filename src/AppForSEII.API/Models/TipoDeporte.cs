using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{

[Index(nameof(NombreTipoDeporte), IsUnique = true)]

public class TipoDeporte
{

[Key]
public int Id { get; set; }

[Required(ErrorMessage = "El nombre del deporte es obligatorio.")]
[StringLength(50, MinimumLength = 3,
ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
public string NombreTipoDeporte { get; set; }

[StringLength(500,
ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
public string? Materiales { get; set; }

public bool Competiciones { get; set; }

// Relación 1:N con Pista
public IList<Pista> Pistas { get; set; }

// Constructor vacío requerido por EF
public TipoDeporte()
{
NombreTipoDeporte = string.Empty;
Pistas = new List<Pista>();
}

// Constructor simple
public TipoDeporte(
string nombreTipoDeporte,
string? materiales,
bool competiciones)
{
NombreTipoDeporte = nombreTipoDeporte;
Materiales = materiales;
Competiciones = competiciones;

Pistas = new List<Pista>();
}
}
}