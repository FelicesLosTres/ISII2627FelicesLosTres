namespace AppForSEII.API.DTOs.InscripcionClaseDeportivaDTOs
{
    public class InscripcionClaseDeportivaDetailDTO 
    {
        public int IdInscripcion { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public decimal PrecioTotal { get; set; }
        public string? MetodoPago { get; set; }

        // Datos recuperados de ApplicationUser
        public string? NombreCliente { get; set; }
        public string? ApellidosCliente { get; set; } 
        public string? Email { get; set; } 
        public string? Telefono { get; set; } 
        public IList<InscripcionClaseDeportivaItemDTO> ClasesInscritas { get; set; } = new List<InscripcionClaseDeportivaItemDTO>();


        public override bool Equals(object? obj)
        {
            return obj is InscripcionClaseDeportivaDetailDTO dto &&
                   IdInscripcion == dto.IdInscripcion &&
                   FechaInscripcion == dto.FechaInscripcion &&
                   PrecioTotal == dto.PrecioTotal &&
                   MetodoPago == dto.MetodoPago &&
                   NombreCliente == dto.NombreCliente &&
                   ApellidosCliente == dto.ApellidosCliente &&
                   Email == dto.Email &&
                   Telefono == dto.Telefono &&
                   ClasesInscritas.SequenceEqual(dto.ClasesInscritas);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(IdInscripcion);
            hashCode.Add(FechaInscripcion);
            hashCode.Add(PrecioTotal);
            hashCode.Add(MetodoPago);
            hashCode.Add(NombreCliente);
            hashCode.Add(ApellidosCliente);
            hashCode.Add(Email);
            hashCode.Add(Telefono);
            foreach (var claseInscrita in ClasesInscritas)
            {
                hashCode.Add(claseInscrita);
            }

            return hashCode.ToHashCode();
        }
    }

}
