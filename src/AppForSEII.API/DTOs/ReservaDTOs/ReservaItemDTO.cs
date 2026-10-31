namespace AppForSEII.API.DTOs.ReservaDTOs
{
    public class ReservaItemDTO
    {
        public ReservaItemDTO(string nombrepista, string nombredeporte, double precio, int npersonas, string observaciones)
        {
            NombrePista = nombrepista;
            NombreDeporte = nombredeporte;
            Precio = precio;
            nPersonas = npersonas;
            Observaciones = observaciones;
        }

        public string NombrePista { get;set; }
        
        public string NombreDeporte { get;set; }

        public double Precio { get;set; }

        public int nPersonas { get;set; }

        public string? Observaciones { get;set; }

        public override bool Equals(object? obj)
        {
                return obj is ReservaItemDTO dTO &&
                NombrePista == dTO.NombrePista &&
                NombreDeporte == dTO.NombreDeporte &&
                Precio == dTO.Precio &&
                nPersonas == dTO.nPersonas &&
                Observaciones == dTO.Observaciones;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombrePista, NombreDeporte, Precio, nPersonas, Observaciones);
        }


    }
}