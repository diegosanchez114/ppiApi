using UPIT.Domain.DTOs;
using UPIT.Domain.Models.Models;
using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.Interfaces.Repositories
{
    public interface IParametricaRepository
    {
        public Task<Parametrica> GetByIdAsync(Guid guid);
        public Task<List<Parametrica>> GetParametricasByName(string Name);
        public Task<DataPaginatedDTO<Parametrica>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(Parametrica model);
        public Task<Guid> UpdateAsync(Guid guid, Parametrica model);
    }
}
