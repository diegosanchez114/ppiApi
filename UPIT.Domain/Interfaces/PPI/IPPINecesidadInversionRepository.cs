using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.Interfaces.PPI
{
    public interface IPPINecesidadInversionRepository
    {
        public Task<PPINecesidadInversion> GetByIdAsync(Guid guid);
        public Task<List<PPINecesidadInversion>> GetAllByIdContractAsync(Guid guid);
        public Task<Guid> CreateAsync(PPINecesidadInversion model);
        public Task<Guid> UpdateAsync(Guid guid, PPINecesidadInversion model);
    }
}
