using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models
{
    public class Competicion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de la competición es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El lugar es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El lugar debe tener entre 3 y 100 caracteres.")]
        public string Lugar { get; set; }

        [Required(ErrorMessage = "El nombre de la competición es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El número de plazas es obligatorio.")]
        [Range(1, 1000, ErrorMessage = "Las plazas deben estar entre 1 y 1000.")]
        public int Plazas { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.00, 99999, ErrorMessage = "El precio no puede ser negativo.")]
        public decimal Precio { get; set; }

        public IList<CompeticionInscrita> CompeticionesInscritas { get; set; }

        public TipoDeporte TipoDeporte { get; set; }
        public Competicion()
        {
        }

      
        public Competicion(DateTime fecha, string lugar, string nombre, int plazas, decimal precio)
        {
            Fecha = fecha;
            Lugar = lugar;
            Nombre = nombre;
            Plazas = plazas;
            Precio = precio;
        }
    }
}