using UPIT.Domain.DTOs;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Domain.Interfaces.ProyectosInternos
{
    public interface IProyectoRepository
    {
        public Task<Proyecto> GetByIdAsync(Guid guid);
        public Task<DataPaginatedDTO<Proyecto>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(Proyecto model);
        public Task<Guid> UpdateAsync(Guid guid, Proyecto model);
    }
}
