using UPIT.Domain.DTOs;
using UPIT.Domain.Models.Prefactibilidad;

namespace UPIT.Domain.Interfaces.Prefactibilidad
{
    public interface IPreProyectoRepository
    {
        public Task<PreProyecto> GetByIdAsync(Guid guid);
        public Task<DataPaginatedDTO<PreProyecto>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(PreProyecto model);
        public Task<Guid> UpdateAsync(Guid guid, PreProyecto model);
    }
}
