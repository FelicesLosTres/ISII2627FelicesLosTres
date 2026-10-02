
[PrimaryKey(nameof(IdAlquiler),nameof(IdMaterial))]
public class MaterialAlquilado
{

    //Constructores
    public MaterialAlquilado()
    {
    }
    public MaterialAlquilado(int cantidad, string descripcion, int idAlquiler, int idMaterial, int precio)
    {
        Cantidad = cantidad;
        Descripcion = descripcion;
        IdAlquiler = idAlquiler;
        IdMaterial = idMaterial;
        Precio = precio;
    }
    
    [Range(1,999,ErrorMessage ="Minimo 1, Máximo 999")]
    public int Cantidad {get; set; }

    [StringLength(200, ErrorMessage ="La descripción tiene un máximo de 200 caracteres.")]
    public string? Descripcion {get; set; }
    [Required]
public int Alquiler { get; set; }

[ForeignKey(nameof(IdAlquiler))]
public Alquiler? Alquiler { get; set; }

[Required]
public int IdMaterial { get; set; }

[ForeignKey(nameof(IdMaterial))]
public Material? Material { get; set; }
  
    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]//Lo que pone antes de .DataType es gen. por el entorno, puede no ser correcto (igual para .Display).
    [System.ComponentModel.DataAnnotations.Display(Name = "Price For Renting whith cuantity")]
    [Precision(5, 2)]
    public int Precio {get; set; }
}