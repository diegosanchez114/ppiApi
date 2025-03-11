using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.DTO.PPI
{
    public class PPIAlerDTO
    {
        public Guid IdContrato { get; set; }
        public string CodigoContrato { get; set; } = string.Empty;
        public Guid IdEntidadResponsable { get; set; }
        public string NombreEntidad { get; set; } = string.Empty;
        public Guid IdAvance { get; set; }
        public DateTime FechaAvance { get; set; }
        public DateTime FechaFinalizacion { get; set; }

    }
}
