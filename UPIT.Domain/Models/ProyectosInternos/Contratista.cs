using System.ComponentModel.DataAnnotations;

namespace UPIT.Domain.Models.ProyectosInternos
{
    public class Contratista
    {
        public Guid IdContratista { get; set; }
        public Guid? IdProyecto { get; set; }
        [Required]
        public string NombreContratista { get; set; } = string.Empty;
        [Required]
        public string NombreAccionistaContratista { get; set; } = string.Empty;
        public decimal? PorcentajeAccionistaContratista { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
