using System.ComponentModel.DataAnnotations;

namespace UPIT.Domain.Models.ProyectosInternos
{
    public class Subdireccion
    {
        public Guid IdSubdireccion { get; set; }
        [Required]
        public string NombreDependencia { get; set; } = string.Empty;
        [Required]
        public string EncargadoDependencia { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
