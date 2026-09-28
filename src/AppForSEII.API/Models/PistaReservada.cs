public class PistaReservada
{
    public int Id{ get; set; }
    public int cantidad { get; set; }
    public int IdPista { get; set; }
    public int IdReserva { get; set; }
    public string Observaciones { get; set;}
    public int Precio { get; set; }

    // Clave foránea y navegación hacia Pista
        public int IdPista { get; set; }
        public virtual Pista? Pista { get; set; }

        // Clave foránea y navegación hacia Reserva
        public int IdReserva { get; set; }
        public virtual Reserva? Reserva { get; set; }

        public PistaReservada() { }

        public override bool Equals(object? obj)
        {
            if (obj is PistaReservada other)
            {
                return ID == other.ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }


}
