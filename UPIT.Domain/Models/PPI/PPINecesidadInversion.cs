namespace UPIT.Domain.Models.PPI
{
    public class PPINecesidadInversion
    {        
        public Guid? IdNecesidad { get; set; }
        public Guid IdContrato { get; set; }
        public decimal ValorInversion { get; set; }
        public string TipoObra { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }    
}
