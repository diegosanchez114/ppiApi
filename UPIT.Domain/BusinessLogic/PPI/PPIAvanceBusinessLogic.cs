using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.BusinessLogic.PPI
{
    public class PPIAvanceBusinessLogic
    {
        public int CalculateDifferenceDays(PPIContrato contrato, PPIAvance avance)
        {
            TimeSpan diffDays = (TimeSpan)(avance.Fecha - contrato.FechaTerminacionContrato)!;
            return diffDays.Days;
        }
    }
}
