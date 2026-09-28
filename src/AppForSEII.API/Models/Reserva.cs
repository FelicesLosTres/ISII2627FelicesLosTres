public class Reserva
{
    public int Id{ get; set; }
    public string Apellidos { get; set; }
    public DateTime FechaReserva { get; set; }
    public string MetodoPago { get; set; }
    public string NombreCliente { get; set;}
    public int PrecioTotal{get; set;}

    // Relación hacia PistasReservadas (1 a muchos)
        public virtual ICollection<PistaReservada> PistasReservadas { get; set; } = new List<PistaReservada>();

        public Reserva() { }

        public override bool Equals(object? obj)
        {
            if (obj is Reserva other)
            {
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
}