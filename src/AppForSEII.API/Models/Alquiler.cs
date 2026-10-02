
using Humanizer;

public class Alquiler
{

    //Constructores
    public Alquiler()
    {
        
    }
    public Alquiler(int idAlquiler, string apellidosUsuario, string nombreUsuario, int dNI, int numeroTelefono, string fechaAlquiler, string metodoPago, decimal precioTotal)
    {
        IdAlquiler = idAlquiler;
        ApellidosUsuario = apellidosUsuario;
        NombreUsuario = nombreUsuario;
        DNI = dNI;
        NumeroTelefono = numeroTelefono;
        FechaAlquiler = fechaAlquiler;
        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
    }

    [Key]
    [RegularExpression (@"^\d{10}$", ErrorMessage ="Id de Alquiler tiene una logitid de 10.")]
    public int IdAlquiler {get; set; }

    [StringLength(40, ErrorMessage ="Los apellidos deben estar comprendidos entre 1 y 40 caracteres.", MinimumLength=1)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
    public string ApellidosUsuario {get; set; }

    [StringLength(20, ErrorMessage ="El nombre debe estar comprendidos entre 1 y 20 caracteres.", MinimumLength=1)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
     public string NombreUsuario {get; set; }

    [RegularExpression(@"^\d{8}[A-Za-z]$")]
    public int DNI {get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
    public int NumeroTelefono {get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    public string FechaAlquiler {get; set; }
    public IList<MaterialAlquilado> MaterialesAlquilados {get; set; }//Para relación con MaterialAlquilado

    [StringLength(40, ErrorMessage ="El campo de método de pago debe estar comprendido entre 1 y 40 caracteres.", MinimumLength=1)]
    public string MetodoPago {get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]//Lo que pone antes de .DataType es gen. por el entorno, puede no ser correcto (igual para .Display).
    [System.ComponentModel.DataAnnotations.Display(Name = "Price For Renting whith cuantity")]
    [Precision(5, 2)]
    public decimal PrecioTotal {get; set; }
}
