
public class TipoMaterial
{
    //Constructores
    public TipoMaterial()
    {
    }
    public TipoMaterial(string nombreTipoMaterial)
    {
        NombreTipoMaterial = nombreTipoMaterial;
    }

    [Key]
    [RegularExpression (@"^\d{10}$", ErrorMessage ="Id de tipo material tiene una logitid de 10.")]
    public int IdTipoMaterial {get; set; }

[Required(ErrorMessage = "El nombre del material es obligatorio.")]
    [StringLength(20, ErrorMessage ="El nombre de un material debe estar comprendido entre 1 y 20 caracteres.", MinimumLength=1)]
    public string NombreTipoMaterial {get; set; }

    public List<Material> Materiales {get; set;}//Para rel. con Material
}