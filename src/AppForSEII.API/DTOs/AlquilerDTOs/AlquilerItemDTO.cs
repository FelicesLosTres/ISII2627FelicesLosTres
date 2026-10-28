
namespace AppForSEII.API.DTOs.AlquilerDTOs
{
    public class AlquilerItemDTO
    {
        public AlquilerItemDTO(int idMaterial, string nombre, string nombreTipoMaterial, string nombreTipoDeporte)
        {
            IdMaterial = idMaterial;
            Nombre = nombre;
            NombreTipoMaterial = nombreTipoMaterial;
            NombreTipoDeprte = nombreTipoDeporte;
        }

        [RegularExpression (@"^\d{10}$", ErrorMessage ="Id de material tiene una logitid de 10.")]
        public int IdMaterial {get; set; }


        [Required(ErrorMessage = "El nombre del material es obligatorio.")]
        [StringLength(20, ErrorMessage = "El nombre de un material debe estar comprendido entre 1 y 20 caracteres.", MinimumLength = 1)]
        public string Nombre {get; set;} = string.Empty;

        public string NombreTipoMaterial {get; set;}
        public string NombreTipoDeprte {get; set;}

        public override bool Equals(object? obj)
        {
            return obj is AlquilerItemDTO dTO &&
                   IdMaterial == dTO.IdMaterial &&
                   Nombre == dTO.Nombre &&
                   NombreTipoMaterial == dTO.NombreTipoMaterial &&
                   NombreTipoDeprte == dTO.NombreTipoDeprte;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdMaterial, Nombre, NombreTipoMaterial, NombreTipoDeprte);
        }
    }
}