

public class Material
{
    [Key]
    [RegularExpression (@"^\d{10}$", ErrorMessage ="Id de material tiene una logitid de 10.")]
    public int IdMaterial {get; set; }

    [Range(1,999,ErrorMessage ="Minimo 1, Máximo 999")]
    public int Cantidad {get; set; }

    [StringLength(20, ErrorMessage ="El nombre de un material debe estar comprendido entre 1 y 20 caracteres.", MinimumLength=1)]
    public required string Nombre {get; set;}//Para evitar errores el entorno me ha recomendado poner "required".

    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]//Lo que pone antes de .DataType es gen. por el entorno, puede no ser correcto (igual para .Display).
    [System.ComponentModel.DataAnnotations.Display(Name = "Price For Renting")]
    [Precision(5, 2)]//5 dígitos y 2 dec.
    public decimal Precio {get; set; }

    //Hacer public IList<MaterialAlquilado> MaterialAlquilado {get; set; } FK.

}