using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
    [PrimaryKey(nameof(CompeticionId), nameof(InscripcionId))]
    public class CompeticionInscrita
    {
        public int CompeticionId { get; set; }
        public Competicion Competicion { get; set; }

        public int InscripcionId { get; set; }
        public Inscripcion Inscripcion { get; set; }

        [StringLength(500, ErrorMessage = "La descripción de problemas físicos no puede superar los 500 caracteres.")]
        public string ProblemasFisicos { get; set; }

        
        public CompeticionInscrita()
        {
        }

       
        public CompeticionInscrita(int competicionId, int inscripcionId, string problemasFisicos)
        {
            CompeticionId = competicionId;
            InscripcionId = inscripcionId;
            ProblemasFisicos = problemasFisicos;
        }
    }
}