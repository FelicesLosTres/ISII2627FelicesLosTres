using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaInscripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string DatosPago { get; set; } = string.Empty;

        [Required]
        public string MetodoPago { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioTotal { get; set; }

        // Clave Foránea (FK) hacia el Cliente (ApplicationUser) 
        public string? ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        public ApplicationUser? Cliente { get; set; }

        //Relación 1:N -> Líneas de clases reservadas en la inscripción 
        public IList<ClaseInscrita> ClasesInscritas { get; set; } = new List<ClaseInscrita>();

        //cosntructor vacío 
        public Inscripcion() { }

    }
}