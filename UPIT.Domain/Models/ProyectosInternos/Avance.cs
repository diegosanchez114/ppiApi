using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPIT.Domain.Models.ProyectosInternos
{
    public class Avance
    {
        public Guid IdAvance { get; set; }
        public Guid? IdProyecto { get; set; }
        [Required]
        public decimal? PorcentajeAvanceTotalPlaneado { get; set; }
        [Required]
        public DateTime? FechaInicio { get; set; }
        [Required]
        public DateTime? FechaFinal { get; set; }
        [Required]
        public DateTime? FechaFinalEfectiva { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
