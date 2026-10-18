namespace PabellonDeportivo.API.DTOs.PistaDTOs
{
    public class PistaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Aforo { get; set; }
        public string NombreDeporte { get; set; } = string.Empty; // "Aplanado" de la entidad Deporte
        public decimal PrecioPorDia { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PistaDTO dto &&
                   Id == dto.Id &&
                   Nombre == dto.Nombre &&
                   Aforo == dto.Aforo &&
                   NombreDeporte == dto.NombreDeporte &&
                   PrecioPorDia == dto.PrecioPorDia;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Aforo, NombreDeporte, PrecioPorDia);
        }
    }
}