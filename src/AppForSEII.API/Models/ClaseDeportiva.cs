using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
    public class ClaseDeportiva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaHora { get; set; }

        [StringLength(100)]
        public string? Lugar { get; set; }

        [Required]
        [StringLength(100)]
        public string Monitor { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Nivel { get; set; } = string.Empty;

        // El caso de uso especifica máximo 30 plazas disponibles
        [Range(0, 30)]
        public int PlazasDisponibles { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        // --- Clave Foránea (FK) hacia TipoDeporte ---
        [Required]
        public int TipoDeporteId { get; set; }

        [ForeignKey(nameof(TipoDeporteId))]
        public TipoDeporte? TipoDeporte { get; set; }

        // --- Relación 1:N -> Registros de inscripción en esta clase ---
        public IList<ClaseInscrita> ClasesInscritas { get; set; } = new List<ClaseInscrita>();

        public ClaseDeportiva() { }

    }
}