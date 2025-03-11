using UPIT.Domain.Models.Prefactibilidad;

namespace UPIT.Domain.Interfaces.Prefactibilidad
{
    public interface IPreTipoAvanceRepository
    {
        public Task<PreTipoAvance> GetByIdAsync(Guid guid);
        public Task<List<PreTipoAvance>> GetAllAsync();
        public Task<Guid> CreateAsync(PreTipoAvance model);
        public Task<Guid> UpdateAsync(Guid guid, PreTipoAvance model);
    }
}
