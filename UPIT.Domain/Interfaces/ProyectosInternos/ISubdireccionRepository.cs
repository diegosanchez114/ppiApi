using System.Data;
using UPIT.Domain.DTOs;
using UPIT.Domain.Models.ProyectosInternos;

namespace UPIT.Domain.Interfaces.ProyectosInternos
{
    public interface ISubdireccionRepository
    {
        public Task<Subdireccion> GetByIdAsync(Guid guid);
        public Task<List<Subdireccion>> GetAllAsync();
    }
}
