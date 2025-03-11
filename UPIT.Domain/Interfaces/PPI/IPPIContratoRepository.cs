using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.Interfaces.PPI
{
    public interface IPPIContratoRepository
    {
        public Task<PPIContrato> GetByIdAsync(Guid guid);
        public Task<List<PPIContrato>> GetAllByIdProjectAsync(Guid guid);
        public Task<List<PPIContrato>> GetAllByIdProjectAndIdEntityAsync(Guid guidProject, Guid guidEntity);
        public Task<Guid> CreateAsync(PPIContrato model);
        public Task<Guid> UpdateAsync(Guid guid, PPIContrato model);
    }
}
