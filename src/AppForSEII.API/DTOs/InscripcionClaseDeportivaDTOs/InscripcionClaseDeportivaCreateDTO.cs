using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.InscripcionClaseDeportivaDTOs
{
    public class InscripcionClaseDeportivaCreateDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una clase para inscribirse.")]
        public List<InscripcionClaseDeportivaItemDTO> ClasesInscritas { get; set; } = new();
    }
}
