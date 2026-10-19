namespace AppForSEII.API.DTOs.InscripcionCompeticionDTOs
{
    public class InscripcionCompeticionDetalleDTO
    {
        public int IdInscripcion { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public decimal PrecioTotal { get; set; }
        public string MetodoPago { get; set; } = string.Empty;

        // Datos recuperados de ApplicationUser
        public string NombreCliente { get; set; } = string.Empty;
        public string ApellidosCliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        // Competiciones de la inscripción
        public List<CompeticionInscritaDetalleDTO> CompeticionesInscritas { get; set; } = new();

        public override bool Equals(object? obj)
        {
            return obj is InscripcionCompeticionDetalleDTO dto &&
                   IdInscripcion == dto.IdInscripcion &&
                   FechaInscripcion == dto.FechaInscripcion &&
                   PrecioTotal == dto.PrecioTotal &&
                   MetodoPago == dto.MetodoPago &&
                   NombreCliente == dto.NombreCliente &&
                   ApellidosCliente == dto.ApellidosCliente &&
                   Email == dto.Email &&
                   Telefono == dto.Telefono &&
                   CompeticionesInscritas.SequenceEqual(dto.CompeticionesInscritas);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdInscripcion, FechaInscripcion, PrecioTotal, MetodoPago,
                NombreCliente, ApellidosCliente, Email, Telefono);
        }
    }

    public class CompeticionInscritaDetalleDTO
    {
        public int CompeticionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoDeporte { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public string? ProblemasFisicos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompeticionInscritaDetalleDTO dto &&
                   CompeticionId == dto.CompeticionId &&
                   Nombre == dto.Nombre &&
                   TipoDeporte == dto.TipoDeporte &&
                   Fecha == dto.Fecha &&
                   Precio == dto.Precio &&
                   ProblemasFisicos == dto.ProblemasFisicos;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompeticionId, Nombre, TipoDeporte, Fecha, Precio, ProblemasFisicos);
        }
    }
}
