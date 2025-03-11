using UPIT.Domain.DTOs;
using UPIT.Domain.Models;

namespace UPIT.Domain.Interfaces
{
    public interface IScopeRepository
    {
        public Task<Scope> GetByIdAsync(Guid guid);
        public Task<DataPaginatedDTO<Scope>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(Scope model);
        public Task<Guid> UpdateAsync(Guid guid, Scope model);
    }
}
