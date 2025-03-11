namespace UPIT.Domain.Models.Prefactibilidad
{
    public class PreTipoAvance
    {
        public Guid IdTipoAvance { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
