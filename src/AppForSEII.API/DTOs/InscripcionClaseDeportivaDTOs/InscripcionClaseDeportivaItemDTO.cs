using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.InscripcionClaseDeportivaDTOs
{
    public class InscripcionClaseDeportivaItemDTO
    {
        [Required]
        public int ClaseDeportivaId { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "Las plazas reservadas deben ser entre 1 y 3 (el usuario y hasta 2 acompañantes).")]
        public int PlazasReservadas { get; set; }

        [StringLength(250)]
        public string? Observaciones { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string TipoDeporte { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public decimal Precio { get; set; }

        public InscripcionClaseDeportivaItemDTO()
        {
        }

        public InscripcionClaseDeportivaItemDTO(int claseDeportivaId, int plazasReservadas, string? observaciones = null)
        {
            ClaseDeportivaId = claseDeportivaId;
            PlazasReservadas = plazasReservadas;
            Observaciones = observaciones;
        }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionClaseDeportivaItemDTO dto &&
                   ClaseDeportivaId == dto.ClaseDeportivaId &&
                   PlazasReservadas == dto.PlazasReservadas &&
                   Observaciones == dto.Observaciones &&
                   Nombre == dto.Nombre &&
                   TipoDeporte == dto.TipoDeporte &&
                   FechaHora == dto.FechaHora &&
                   Precio == dto.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ClaseDeportivaId,
                PlazasReservadas,
                Observaciones,
                Nombre,
                TipoDeporte,
                FechaHora,
                Precio);
        }
    }
}