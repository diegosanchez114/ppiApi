using UPIT.Domain.DTOs;
using UPIT.Domain.Models;
using UPIT.Domain.Models.Prefactibilidad;

namespace UPIT.Domain.Interfaces.Prefactibilidad
{
    public interface IPreAvanceRepository
    {
        public Task<DataPaginatedDTO<PreAvance>> GetByFilterPaginatedAsync(string id, int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(PreAvance model);
        public Task<Guid> UpdateAsync(Guid guid, PreAvance model);
    }
}
