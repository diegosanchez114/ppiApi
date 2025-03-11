using UPIT.Domain.DTO.PPI;
using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.Interfaces.PPI
{
    public interface IPPIAvanceRepository
    {
        public Task<PPIAvance> GetByIdAsync(Guid guid);
        public Task<List<PPIAvanceDTO>> GetAllByIdContractAsync(Guid guid);
        public Task<Guid> CreateAsync(PPIAvance model);
        public Task<Guid> UpdateAsync(Guid guid, PPIAvance model);
    }
}
