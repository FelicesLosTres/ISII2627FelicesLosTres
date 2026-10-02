
using Humanizer;

public class Alquiler
{

    //Constructores
    public Alquiler()
    {
        
    }
    public Alquiler(  string fechaAlquiler, MetodoPago metodoPago, decimal precioTotal, ApplicationUser applicationUser)
    {
      
        FechaAlquiler = fechaAlquiler;
        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
        ApplicationUser = applicationUser;
    }

    [Key]
    [RegularExpression (@"^\d{10}$", ErrorMessage ="Id de Alquiler tiene una logitid de 10.")]
    public int IdAlquiler {get; set; }

// FK con ApplicationUser
[Required]
public string UserId { get; set; } 

[ForeignKey(nameof(UserId))]
public ApplicationUser ApplicationUser { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    public string FechaAlquiler {get; set; }

    [StringLength(40, ErrorMessage ="El campo de método de pago debe estar comprendido entre 1 y 40 caracteres.", MinimumLength=1)]
    public MetodoPago MetodoPago {get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]//Lo que pone antes de .DataType es gen. por el entorno, puede no ser correcto (igual para .Display).
    [System.ComponentModel.DataAnnotations.Display(Name = "Price For Renting whith cuantity")]
    [Precision(5, 2)]
    public decimal PrecioTotal {get; set; }

    public IList<MaterialAlquilado> MaterialesAlquilados {get; set;}//Para rel. con MaterialAlquilado
}
