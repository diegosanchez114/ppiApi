using UPIT.Domain.Models.Transversales;

namespace UPIT.Domain.Interfaces.Transversales
{
    public interface IEntidadRepository
    {
        public Task<Entidad> GetByIdAsync(Guid guid);
        public Task<List<Entidad>> GetAllAsync();
        public Task<Guid> CreateAsync(Entidad model);
        public Task<Guid> UpdateAsync(Guid guid, Entidad model);
    }
}
