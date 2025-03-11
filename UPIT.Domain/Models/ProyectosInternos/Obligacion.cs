using System.ComponentModel.DataAnnotations;

namespace UPIT.Domain.Models.ProyectosInternos
{
    public class Obligacion
    {
        public Guid IdObligacion { get; set; }
        [Required]
        public Guid? IdProyecto { get; set; }
        public Guid? IdContratista { get; set; }
        public Guid? IdInterventoria { get; set; }
        public int? TipoObligacion { get; set; }
        public int? Tiempo { get; set; }
        public decimal? PorcentajeAvancePlaneado { get; set; }
        public decimal? PorcentajeAvanceEjecutado { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
