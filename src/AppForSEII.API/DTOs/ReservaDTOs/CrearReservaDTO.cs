using System.ComponentModel.DataAnnotations;

namespace PabellonDeportivo.API.DTOs.ReservaDTOs
{
    public class CrearReservaDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [RegularExpression(@"^[0-9]{8}[A-Z]$")]
        public string DNI { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Phone]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione un método de pago")]
        public string MetodoPago { get; set; } = string.Empty; // "Bizum", "Tarjeta", etc.

        [Required(ErrorMessage = "Introduzca los datos del pago (ej. nº tarjeta o móvil)")]
        public string DatosPago { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "El carrito no puede estar vacío")]
        public List<ItemReservaDTO> PistasSeleccionadas { get; set; } = new();
    }

    public class ItemReservaDTO
    {
        [Required]
        public int PistaId { get; set; }

        [MaxLength(500)]
        public string? Observaciones { get; set; }
    }
}