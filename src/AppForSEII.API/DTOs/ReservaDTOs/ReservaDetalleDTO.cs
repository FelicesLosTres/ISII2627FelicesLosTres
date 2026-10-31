using Microsoft.Identity.Client;

namespace AppForSEII.API.DTOs.ReservaDTOs
{
    public class ReservaDetalleDTO : CrearReservaDTO
    {
        public ReservaDetalleDTO(int id, DateTime fechareserva, string nombre,string apellido, string correo, string telefono, double preciototal, MetodoPago metodopago, IList<ReservaItemDTO> pistasreservadas)
        : base(pistasreservadas, nombre, apellido, correo, telefono, metodopago)
        {
            IdReserva = id;
            FechaReserva = fechareserva;
        }

        public int IdReserva { get; set; }
        public DateTime FechaReserva { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaDetalleDTO dTO &&
            base.Equals(obj) &&
            PrecioTotal == dTO.PrecioTotal &&
            IdReserva == dTO.IdReserva &&
            CompareDate(FechaReserva, dTO.FechaReserva);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdReserva, FechaReserva);
        }

}
}