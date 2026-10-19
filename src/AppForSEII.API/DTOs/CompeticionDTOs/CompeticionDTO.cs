namespace AppForSEII.API.DTOs.CompeticionDTOs
{
    public class CompeticionDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoDeporte { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int PlazasLibres { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompeticionDTO dto &&
                   Id == dto.Id &&
                   Nombre == dto.Nombre &&
                   TipoDeporte == dto.TipoDeporte &&
                   Fecha == dto.Fecha &&
                   Lugar == dto.Lugar &&
                   Precio == dto.Precio &&
                   PlazasLibres == dto.PlazasLibres;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, TipoDeporte, Fecha, Lugar, Precio, PlazasLibres);
        }
    }
}
