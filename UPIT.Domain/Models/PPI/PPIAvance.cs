namespace UPIT.Domain.Models.PPI
{
    public class PPIAvance
    {
        public Guid? IdAvance { get; set; }
        public Guid IdContrato { get; set; }
        public DateTime Fecha { get; set; }
        public decimal PorcentajeProgramado { get; set; }
        public decimal PorcentajeEjecutado { get; set; }
        public decimal? ValorEjecutado { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public bool? TieneAlerta { get; set; } = false;
        public bool? AlertaResuelta { get; set; } = false;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
