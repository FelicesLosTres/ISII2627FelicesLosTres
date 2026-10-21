using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII.API.DTOs.MaterialDTOs
{
    
    public class MaterialDTO
    {
        //Constructores
        public MaterialDTO()
        {
        }
        public MaterialDTO(int idMaterial, string nombre, string tipoMaterial, string tipoDeporte, decimal precio, int cantidad)
        {
            IdMaterial = idMaterial;
            Nombre = nombre;
            TipoMaterial = tipoMaterial;
            TipoDeporte = tipoDeporte;
            Precio = precio;
            Cantidad = cantidad;
        }

        [RegularExpression (@"^\d{10}$", ErrorMessage ="Id de material tiene una logitid de 10.")]
        public int IdMaterial { get; set; }

        [Required(ErrorMessage = "El nombre del material es obligatorio.")]
        [StringLength(20, ErrorMessage = "El nombre de un material debe estar comprendido entre 1 y 20 caracteres.", MinimumLength = 1)]
        public string Nombre { get; set; } = string.Empty;


        public string? TipoMaterial { get; set; } = string.Empty;
        public string? TipoDeporte { get; set; } = string.Empty; // "Aplanado" de la entidad Deporte

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]//Lo que pone antes de .DataType es gen. por el entorno, puede no ser correcto (igual para .Display).
        [System.ComponentModel.DataAnnotations.Display(Name = "Price For Renting")]
        [Precision(5, 2)]
        public decimal Precio { get; set; } = 0;

        [Range(1,999,ErrorMessage ="Minimo 1, Máximo 999")]
        public int Cantidad {get; set; }

        public override bool Equals(object? obj)
        {
            return obj is MaterialDTO dto &&
                IdMaterial == dto.IdMaterial &&
                   Nombre == dto.Nombre &&
                   TipoMaterial == dto.TipoMaterial &&
                   TipoDeporte == dto.TipoDeporte &&
                   Precio == dto.Precio &&
                   Cantidad == dto.Cantidad;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(IdMaterial, Nombre, TipoMaterial, TipoDeporte, Precio, Cantidad);
            //throw new NotImplementedException();
        }
    }
}