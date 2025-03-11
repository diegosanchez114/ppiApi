using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.DTO.PPI
{
    public class PPIProyectoDTO: PPIProyecto
    {
        public string NombreEntidad { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public string NombreModo { get; set; } = string.Empty;
        public string NombrePrograma {  get; set; } = string.Empty;
        public string NombrePriorizacionInvias {  get; set; } = string.Empty;
        public double TotalValorProyecto { get; set; }
        public double TotalPorcentajeEjecutado { get; set; }
        public string TotalValorEjecutado { get; set; } = string.Empty;
        public int CantContratos { get; set; }
    }
}
