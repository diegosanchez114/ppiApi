using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPIT.Domain.Models.ProyectosInternos
{
    public class Proyecto
    {
        public Guid? IdProyecto { get; set; }
        //[Required]
        public Guid? IdSubdireccion { get; set; }
        //[Required]
        public string NombreProyecto { get; set; } = string.Empty;
        //[Required]
        public string ObjetoProyecto { get; set; } = string.Empty;
        //[Required]
        public DateTime? FechaInicioContrato { get; set; }
        [Required]
        public DateTime? FechaFinalContratoContractual { get; set; }
        public DateTime? FechaFinalRealContrato { get; set; }
        public DateTime? FechaSuscripcionContrato { get; set; }
        //public decimal? PorcentajeTotalAvancePlaneado { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public decimal? ValorContrastistaInicial { get; set; }
        public decimal? ValorInterventoriaInicial { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
