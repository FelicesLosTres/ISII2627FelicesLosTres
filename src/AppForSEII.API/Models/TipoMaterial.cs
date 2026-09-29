
public class TipoMaterial
{
    
    [Key]
    [RegularExpression (@"^\d{10}$", ErrorMessage ="Id de tipo material tiene una logitid de 10.")]
    public int IdTipoMaterial {get; set; }

    [StringLength(20, ErrorMessage ="El nombre de un material debe estar comprendido entre 1 y 20 caracteres.", MinimumLength=1)]
    public required string NombreTipoMaterial {get; set; }
}