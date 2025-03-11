namespace UPIT.Domain.Models.PPI
{
    public class PPIContrato
    {
        public Guid? IdContrato { get; set; }
        public Guid IdProyecto { get; set; }
        public Guid IdEntidadResponsable { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Objeto { get; set; } = string.Empty;
        public DateTime? FechaInicioContrato { get; set; }
        public DateTime? FechaTerminacionContrato { get; set; }
        public decimal ValorContrato { get; set; }        
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
