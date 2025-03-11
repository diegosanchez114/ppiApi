namespace UPIT.Domain.Models.Prefactibilidad
{
    public class PreAvance
    {
        public Guid? IdAvance { get; set; }
        public Guid IdProyecto { get; set; }
        public Guid IdTipoAvance { get; set; }
        public int NumAvance { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public DateTime FechaAvance { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
