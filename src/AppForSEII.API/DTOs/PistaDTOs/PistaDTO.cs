namespace AppForSEII.API.DTOs.PistaDTOs
{
    public class PistaDTO
    {
        public int IdPista { get; set; }
        public string NombrePista { get; set; } = string.Empty;
        public int NPersonas { get; set; }
        public string TipoDeporte { get; set; } = string.Empty; // "Aplanado" de la entidad Deporte
        public decimal Precio { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PistaDTO dto &&
                IdPista == dto.IdPista &&
                   NombrePista == dto.NombrePista &&
                   NPersonas == dto.NPersonas &&
                   TipoDeporte == dto.TipoDeporte &&
                   Precio == dto.Precio;
        }
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}