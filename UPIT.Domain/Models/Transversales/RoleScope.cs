using System.ComponentModel.DataAnnotations;

namespace UPIT.Domain.Models
{
    public class RoleScope
    {
        public Guid RoleScopeId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public Guid ScopeId { get; set; }
        public DateTime RoleScopeCreated { get; set; }
        public DateTime RoleScopeUpdated { get; set; }
    }
}
