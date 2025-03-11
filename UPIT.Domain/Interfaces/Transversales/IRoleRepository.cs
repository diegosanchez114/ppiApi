using UPIT.Domain.Models;

namespace UPIT.Domain.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Role> GetByIdAsync(Guid guid);
        public Task<List<Role>> GetAllAsync();
        public Task<Guid> CreateAsync(Role model);
        public Task<Guid> UpdateAsync(Guid guid, Role model);
    }
}
