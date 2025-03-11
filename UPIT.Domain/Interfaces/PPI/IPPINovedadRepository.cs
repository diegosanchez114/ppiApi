using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.Interfaces.PPI
{
    public interface IPPINovedadRepository
    {
        public Task<PPINovedad> GetByIdAsync(Guid guid);
        public Task<List<PPINovedad>> GetAllByIdAvanceAsync(Guid guid);
        public Task<Guid> CreateAsync(PPINovedad model);
        public Task<Guid> UpdateAsync(Guid guid, PPINovedad model);
    }
}
