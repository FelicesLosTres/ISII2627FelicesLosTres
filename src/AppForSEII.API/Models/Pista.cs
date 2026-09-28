public class Pista
{
    public int IdPista { get; set; }
    public string NombrePista { get; set; }
    public string NPersonas { get; set; }
    public int Precio { get; set; }
    public int Stock { get; set; }


 // Clave foránea y relación con TipoDeporte
        public int TipoDeporteId { get; set; }
        public virtual TipoDeporte? TipoDeporte { get; set; }

        // Relación con PistasReservadas (1 a muchos)
        public virtual ICollection<PistaReservada> PistasReservadas { get; set; } = new List<PistaReservada>();

        public Pista() { }

        public override bool Equals(object? obj)
        {
            if (obj is Pista other)
            {
                return IdPista == other.IdPista;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return IdPista.GetHashCode();
        }

}