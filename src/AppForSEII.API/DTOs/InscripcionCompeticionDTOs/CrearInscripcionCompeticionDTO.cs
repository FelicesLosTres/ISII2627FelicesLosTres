using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.InscripcionCompeticionDTOs
{
    public class CrearInscripcionCompeticionDTO
    {
        [Required, EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una competición para inscribirse.")]
        public List<ItemInscripcionCompeticionDTO> CompeticionesSeleccionadas { get; set; } = new();
    }

    public class ItemInscripcionCompeticionDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "La competición seleccionada no es válida.")]
        public int CompeticionId { get; set; }

        [StringLength(250, ErrorMessage = "Los problemas físicos no pueden superar los 250 caracteres.")]
        public string? ProblemasFisicos { get; set; }
    }
}
