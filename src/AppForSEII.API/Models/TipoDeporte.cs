public class TipoDeporte
{
    public int Id { get; set; }
    public string Competiciones { get; set; }
    public string Materiales{ get; set; }
    public string Nombre { get; set; }
    public string NombreTipoDeporte{ get; set;}

    // Relación inversa con Pista
    public virtual ICollection<Pista> Pistas { get; set; } = new List<Pista>();

        public TipoDeporte() { }

        public override bool Equals(object? obj)
        {
            if (obj is TipoDeporte other)
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