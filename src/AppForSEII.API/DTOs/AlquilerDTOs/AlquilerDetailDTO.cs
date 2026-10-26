
namespace AppForSEII.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetailDTO : AlquilerForCreateDTO
    {
        //Constructor
        public AlquilerDetailDTO(int idAlquiler, decimal precioTotal, DateTime fechaAlquiler, int usuarioDNI, string nombreUsuario, string apellidosUsuario,
                    string correoElectronicoUsuario, int telefonoUsuario,
                    MetodoPago metodoPago, IList<AlquilerItemDTO> materialesAlquilados)
            //Para llamar al constructor de AlquilerForCreateDTO. No usado porque aún no está implementado
            //: base(idAlquiler,precioTotal,fechaAlquiler,usuarioDNI,nombreUsuario,apellidosUsuario,correoElectronicoUsuario,telefonoUsuario,metodoPago,alquilerItems)
        {
            IdAlquiler = idAlquiler;
            PrecioTotal = precioTotal;
            FechaAlquiler = fechaAlquiler;
            UsuarioDNI = usuarioDNI;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            CorreoElectronicoUsuario = correoElectronicoUsuario;
            TelefonoUsuario = telefonoUsuario;
            MetodoPago = metodoPago;
            MaterialesAlquilados = materialesAlquilados ?? throw new ArgumentNullException(nameof(materialesAlquilados));

        }

        [RegularExpression (@"^\d{10}$", ErrorMessage ="Id de Alquiler tiene una logitid de 10.")]
         public int IdAlquiler {get; set; }

        [Required(ErrorMessage = "La fecha de la reserva es obligatoria.")]
        public DateTime FechaAlquiler {get; set; }

        [StringLength(40, ErrorMessage ="El campo de método de pago debe estar comprendido entre 1 y 40 caracteres.", MinimumLength=1)]
        public MetodoPago MetodoPago {get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [System.ComponentModel.DataAnnotations.Display(Name = "Price For Renting whith cuantity")]
        [Precision(5, 2)]
        public decimal PrecioTotal {get; set; }

        public IList<AlquilerItemDTO> MaterialesAlquilados {get; set;} = new List<AlquilerItemDTO>();

        public int UsuarioDNI {get; set;}
        public string NombreUsuario {get; set;}
        public string ApellidosUsuario{get; set;}

        [EmailAddress]
        public string CorreoElectronicoUsuario{get; set;}

        [Phone]
        public int TelefonoUsuario{get; set;}


        public override bool Equals(object? obj)
        {
            return obj is AlquilerDetailDTO dTO &&
                   base.Equals(obj) &&
                   PrecioTotal == dTO.PrecioTotal &&
                   IdAlquiler == dTO.IdAlquiler &&
                   CompareDate(FechaAlquiler, dTO.FechaAlquiler);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdAlquiler, FechaAlquiler);
        }
    }
}