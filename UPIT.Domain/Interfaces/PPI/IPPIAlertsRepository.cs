using UPIT.Domain.DTO.PPI;
using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.Interfaces.PPI
{
    public interface IPPIAlertsRepository
    {
        public Task<List<PPIAlerDTO>> getAlertsAvances();
    }
}
