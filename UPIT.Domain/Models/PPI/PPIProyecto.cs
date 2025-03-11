namespace UPIT.Domain.Models.PPI
{
    public class PPIProyecto
    {
        public Guid? IdProyecto { get; set; }
        public Guid? IdEntidadResponsable { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string NombreLargo { get; set; } = string.Empty;
        public string NombreCorto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Modo { get; set; } = string.Empty;
        public bool EstaRadicado { get; set; }
        public string CodigoCorredor { get; set; } = string.Empty;
        public bool TieneEstudiosYDisenio { get; set; }
        public string Programa { get; set; } = string.Empty;
        public string PriorizacionInvias { get; set; } = string.Empty;
        public string BpinPng { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public bool TieneContratos { get; set; }
        public string TieneContratosObservacion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
