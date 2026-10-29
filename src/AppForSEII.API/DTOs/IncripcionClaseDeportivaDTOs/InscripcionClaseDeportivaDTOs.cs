namespace AppForSEII.API.DTOs.ClaseDeportivaDTOs
{
    public class InscripcionClaseDeportivaDTO
    {
        public InscripcionClaseDeportivaDTO()
        {
        }

        public InscripcionClaseDeportivaDTO(int idClaseDeportiva, DateTime fechaHora, string? lugar, string monitor, string nivel,
            int plazasDisponibles, decimal precioUnitario, string nombre, string descripcion, int duracion, string tipoDeporte)
        {
            IdClaseDeportiva = idClaseDeportiva;
            FechaHora = fechaHora;
            Lugar = lugar;
            Monitor = monitor;
            Nivel = nivel;
            PlazasDisponibles = plazasDisponibles;
            PrecioUnitario = precioUnitario;
            Nombre = nombre;
            Descripcion = descripcion;
            Duracion = duracion;
            TipoDeporte = tipoDeporte;
        }

        public int IdClaseDeportiva { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Lugar { get; set; }
        public string Monitor { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public int PlazasDisponibles { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public string TipoDeporte { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is InscripcionClaseDeportivaDTO dto &&
                   IdClaseDeportiva == dto.IdClaseDeportiva &&
                   FechaHora == dto.FechaHora &&
                   Lugar == dto.Lugar &&
                   Monitor == dto.Monitor &&
                   Nivel == dto.Nivel &&
                   PlazasDisponibles == dto.PlazasDisponibles &&
                   PrecioUnitario == dto.PrecioUnitario &&
                   Nombre == dto.Nombre &&
                   Descripcion == dto.Descripcion &&
                   Duracion == dto.Duracion &&
                   TipoDeporte == dto.TipoDeporte;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                IdClaseDeportiva,
                FechaHora,
                Lugar,
                Monitor,
                Nivel,
                PlazasDisponibles,
                PrecioUnitario,
                HashCode.Combine(Nombre, Descripcion, Duracion, TipoDeporte));
        }
    }
}