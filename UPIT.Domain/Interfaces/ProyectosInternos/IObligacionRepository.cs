using UPIT.Domain.DTOs;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Domain.Interfaces.ProyectosInternos
{
    public interface IObligacionRepository
    {
        public Task<Obligacion> GetByIdAsync(Guid guid);
        public Task<DataPaginatedDTO<Obligacion>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(Obligacion model);
        public Task<Guid> UpdateAsync(Guid guid, Obligacion model);
    }
}
