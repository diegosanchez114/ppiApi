using UPIT.Domain.DTOs;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Domain.Interfaces.ProyectosInternos
{
    public interface IAvanceRepository
    {
        public Task<Avance> GetByIdAsync(Guid guid);
        public Task<DataPaginatedDTO<Avance>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(Avance model);
        public Task<Guid> UpdateAsync(Guid guid, Avance model);
    }
}
