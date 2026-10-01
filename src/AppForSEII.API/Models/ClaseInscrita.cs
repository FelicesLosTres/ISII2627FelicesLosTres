using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
    public class ClaseInscrita
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        // El flujo basico del CU indica que dispone de 2 plazas para acompañantes (máximo 3 en total)
        [Range(1, 3)]
        public int PlazasReservadas { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        // --- Clave Foránea (FK) hacia ClaseDeportiva
        [Required]
        public int ClaseDeportivaId { get; set; }

        [ForeignKey(nameof(ClaseDeportivaId))]
        public ClaseDeportiva? ClaseDeportiva { get; set; }

        // --- Clave Foránea (FK) hacia Inscripcion 
        [Required]
        public int InscripcionId { get; set; }

        [ForeignKey(nameof(InscripcionId))]
        public Inscripcion? Inscripcion { get; set; }

        //constructor vacío 
        public ClaseInscrita() { }

        public ClaseInscrita(string? observaciones, int plazasReservadas, decimal precio, int claseDeportivaId, int inscripcionId)
        {
            Observaciones = observaciones;
            PlazasReservadas = plazasReservadas;
            Precio = precio;
            ClaseDeportivaId = claseDeportivaId;
            InscripcionId = inscripcionId;
        }
    }
        
}