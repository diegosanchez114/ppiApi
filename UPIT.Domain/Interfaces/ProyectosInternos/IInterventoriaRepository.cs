using UPIT.Domain.DTOs;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Domain.Interfaces.ProyectosInternos
{
    public interface IInterventoriaRepository
    {
        public Task<Interventoria> GetByIdAsync(Guid guid);
        public Task<DataPaginatedDTO<Interventoria>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
    }
}
