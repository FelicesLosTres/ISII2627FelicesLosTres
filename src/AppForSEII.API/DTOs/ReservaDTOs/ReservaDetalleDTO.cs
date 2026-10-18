namespace PabellonDeportivo.API.DTOs.ReservaDTOs
{
    public class ReservaDetalleDTO
    {
        public int Id { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public decimal PrecioTotal { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string ApellidosCliente { get; set; } = string.Empty;
        public string DNI { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;

        public List<PistaReservaDetalleDTO> PistasAlquiladas { get; set; } = new();
    }

    public class PistaReservaDetalleDTO
    {
        public string NombrePista { get; set; } = string.Empty;
        public string Deporte { get; set; } = string.Empty;
        public int Aforo { get; set; }
        public string? Observaciones { get; set; }
    }
}
