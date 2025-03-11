namespace UPIT.Domain.Models.PPI
{
    public class PPINovedad
    {
        public Guid? IdNovedad { get; set; }
        public Guid IdAvance { get; set; }
        public DateTime Fecha { get; set; }        
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
