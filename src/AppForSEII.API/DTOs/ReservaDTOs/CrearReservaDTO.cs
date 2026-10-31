using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.ReservaDTOs
{
    public class CrearReservaDTO
    {
        public CrearReservaDTO(IList<ReservaItemDTO> pistasreservadas, string nombre, string apellido, string correo, string telefono, MetodoPago metodopago)
        {
            NombreCliente = nombre ?? throw new ArgumentNullException(nameof(NombreCliente));
            ApellidosCliente = apellido ?? throw new ArgumentNullException(nameof(ApellidosCliente));
            CorreoCliente = correo ?? throw new ArgumentNullException(nameof(CorreoCliente));
            numTelefono = telefono ?? throw new ArgumentNullException(nameof(numTelefono));
            metodo = metodopago;
            PistasSeleccionadas = pistasreservadas ?? throw new ArgumentNullException(nameof(PistasSeleccionadas));

        }

        public CrearReservaDTO()
        {
            PistasSeleccionadas = new List<ReservaItemDTO>();
        }

        public IList<ReservaItemDTO> PistasSeleccionadas { get;set; } = new List<ReservaItemDTO>();

        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, escriba su Nombre")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El nombre tiene que tener como mínimo 1 caracteres")]
        public string NombreCliente { get; set; }= "Peter Jackson";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, escriba sus Apellidos")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Los apellidos tienen que tener como mínimo 1 caracteres")]
        public string ApellidosCliente { get; set; }= "Jackson";



        [Required]
        [EmailAddress]
        public string CorreoCliente { get;set; } = "peter.jackson@uclm.es";

        public string numTelefono { get;set; } = "234";


        [Required]
        public MetodoPago metodo { get;set; } = MetodoPago.Efectivo;

        public double PrecioTotal
        {
            get
            {
                return PistasSeleccionadas.Sum(pr => pr.Precio);
            }
        }

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }

        public override bool Equals(object? obj)
        {
            return obj is CrearReservaDTO dTO &&
            PistasSeleccionadas.SequenceEqual(dTO.PistasSeleccionadas) &&
            NombreCliente == dTO.NombreCliente &&
            ApellidosCliente == dTO.ApellidosCliente &&
            CorreoCliente == dTO.CorreoCliente &&
            numTelefono == dTO.numTelefono &&
            metodo == dTO.metodo &&
            PrecioTotal == dTO.PrecioTotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), CorreoCliente);
        }





    }

}