using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
    [PrimaryKey(nameof(CompeticionId), nameof(InscripcionId))]
public class CompeticionInscrita
{
[Required]
public int CompeticionId { get; set; }

[ForeignKey(nameof(CompeticionId))]
public Competicion? Competicion { get; set; }

[Required]
public int InscripcionId { get; set; }

[ForeignKey(nameof(InscripcionId))]
public Inscripcion? Inscripcion { get; set; }

[StringLength(250)]
public string ProblemasFisicos { get; set; } = string.Empty;

public CompeticionInscrita()
{
}

public CompeticionInscrita(
int competicionId,
int inscripcionId,
string problemasFisicos)
{
CompeticionId = competicionId;
InscripcionId = inscripcionId;
ProblemasFisicos = problemasFisicos;
}
}
}