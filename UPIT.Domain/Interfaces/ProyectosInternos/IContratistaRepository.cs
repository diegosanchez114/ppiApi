using UPIT.Domain.DTOs;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Domain.Interfaces.ProyectosInternos
{
    public interface IContratistaRepository
    {
        public Task<Contratista> GetByIdAsync(Guid guid);
        public Task<DataPaginatedDTO<Contratista>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
    }

}
