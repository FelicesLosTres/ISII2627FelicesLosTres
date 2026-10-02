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

[ForeignKey(nameof(ClienteId))]
public ApplicationUser? Cliente { get; set; }

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
using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models
{
    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden superar los 100 caracteres.")]
        public string ApellidosUsuario { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El DNI debe tener 9 caracteres.")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "La fecha de inscripción es obligatoria.")]
        public DateTime FechaInscripcion { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public string MetodoPago { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El precio total es obligatorio.")]
        [Range(0.00, 99999, ErrorMessage = "El precio no puede ser negativo.")]
        public decimal PrecioTotal { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Telefono { get; set; }

        public IList<CompeticionInscrita> CompeticionesInscritas { get; set; }
        public Inscripcion()
        {
        }

      
        public Inscripcion(string apellidosUsuario, string dni, DateTime fechaInscripcion, string metodoPago, string nombreUsuario, decimal precioTotal, string telefono)
        {
            ApellidosUsuario = apellidosUsuario;
            DNI = dni;
            FechaInscripcion = fechaInscripcion;
            MetodoPago = metodoPago;
            NombreUsuario = nombreUsuario;
            PrecioTotal = precioTotal;
            Telefono = telefono;
        }
    }
}