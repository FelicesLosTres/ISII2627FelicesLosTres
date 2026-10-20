namespace AppForSEII.API.DTOs.ReservaDTOs
{
    public class ReservaDetalleDTO
    {
        public ReservaDetalleDTO(int idReserva, DateTime fechaReserva, decimal precioTotal, string v, string? name, string? surname, string? email, string? phoneNumber, List<PistaReservadaDetalleDTO> pistaReservadaDetalleDTOs)
        {
            IdReserva = idReserva;
            FechaReserva = fechaReserva;
            PrecioTotal = precioTotal;
            Email = email;
        }

        public int IdReserva { get; set; }
        public DateTime FechaReserva { get; set; }
        public decimal PrecioTotal { get; set; }
        public string MetodoPago { get; set; } = string.Empty;

        // Datos recuperados de ApplicationUser
        public string NombreCliente { get; set; } = string.Empty;
        public string ApellidosCliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        // Pistas de la reserva
        public List<PistaReservadaDetalleDTO> PistasReservadas { get; set; } = new();
    }

    public class PistaReservadaDetalleDTO
    {
        public int PistaId { get; set; }
        public string NombrePista { get; set; } = string.Empty;
        public string NombreDeporte { get; set; } = string.Empty;
        public int Aforo { get; set; }
        public decimal Precio { get; set; }
        public string? Observaciones { get; set; }
    }
}