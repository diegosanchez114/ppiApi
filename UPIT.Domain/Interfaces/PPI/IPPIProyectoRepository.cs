using UPIT.Domain.DTO.PPI;
using UPIT.Domain.DTOs;
using UPIT.Domain.Models.PPI;

namespace UPIT.Domain.Interfaces.PPI
{
    public interface IPPIProyectoRepository
    {
        public Task<PPIProyectoDTO> GetByIdAsync(Guid guid);
        public Task<DataPaginatedDTO<PPIProyectoDTO>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
        public Task<DataPaginatedDTO<PPIProyectoDTO>> GetByEntityFilterPaginatedAsync(Guid id, int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(PPIProyecto model);
        public Task<Guid> UpdateAsync(Guid guid, PPIProyecto model);
    }
}
