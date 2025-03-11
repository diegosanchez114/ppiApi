using UPIT.Domain.DTOs;
using UPIT.Domain.Models;

namespace UPIT.Domain.Interfaces
{
    public interface IRoleScopeRepository
    {
        public Task<RoleScope> GetByIdAsync(Guid guid);
        public Task<List<RoleScope>> GetScopesByRolAsync(Guid guid);
        public Task<Guid> CreateAsync(RoleScope model);
        public Task<Guid> UpdateAsync(Guid guid, RoleScope model);
        public Task<Guid> DeleteAsync(Guid guid);
    }
}
