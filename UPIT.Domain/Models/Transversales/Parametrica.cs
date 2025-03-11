namespace UPIT.Domain.Models.Models
{
    public class Parametrica
    {
        public Guid? Id { get; set; }
        public string Nombre { get; set; } = String.Empty;
        public string Valor { get; set; } = String.Empty;
        public string Descripcion { get; set; } = String.Empty;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
