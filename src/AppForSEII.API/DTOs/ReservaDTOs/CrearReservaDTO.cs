using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.ReservaDTOs
{
    public class CrearReservaDTO
    {
        [Required, EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una pista para reservar.")]
        public List<ItemReservaDTO> PistasSeleccionadas { get; set; } = new();
    }

    public class ItemReservaDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "La pista seleccionada no es válida.")]
        public int PistaId { get; set; }

        [StringLength(250, ErrorMessage = "Las observaciones no pueden superar los 250 caracteres.")]
        public string? Observaciones { get; set; }
    }
}