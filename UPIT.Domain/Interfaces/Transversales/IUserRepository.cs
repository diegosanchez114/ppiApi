using UPIT.Domain.DTOs;
using UPIT.Domain.Models;

namespace UPIT.Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetByIdAsync(Guid guid);
        public Task<User> GetByEmailAsync(string email);
        public Task<DataPaginatedDTO<User>> GetByFilterPaginatedAsync(int? page, int? limit, string? filter);
        public Task<Guid> CreateAsync(User model);
        public Task<Guid> UpdateAsync(Guid guid, User model);
        public Task<Guid> UpdateStateAsync(Guid guid, bool state);
    }
}
