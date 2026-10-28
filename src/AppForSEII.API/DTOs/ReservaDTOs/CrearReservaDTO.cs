using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.ReservaDTOs
{
    public class CrearReservaDTO
    {
        public CrearReservaDTO(IList<ReservaItemDTO> pistasreservadas, string nombreapellidocliente, string correo, int telefono, MetodoPago metodopago)
        {
            PistasSeleccionadas = pistasreservadas;
            NombreApellidosCliente = nombreapellidocliente;
            CorreoCliente = correo;
            numTelefono = telefono;
            metodo = metodopago;

        }

        public CrearReservaDTO()
        {
            PistasSeleccionadas = new List<ReservaItemDTO>();
        }

        public IList<ReservaItemDTO> PistasSeleccionadas { get;set; } = new List<ReservaItemDTO>();

        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, escriba su Nombre y Apellidos")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El nombre y apellidos tienen que tener como mínimo 10 caracteres")]
        public string NombreApellidosCliente { get; set; }= "Peter Jackson";

        [Required]
        [EmailAddress]
        public string CorreoCliente { get;set; } = "peter.jackson@uclm.es";

        public int numTelefono { get;set; }


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
            NombreApellidosCliente == dTO.NombreApellidosCliente &&
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