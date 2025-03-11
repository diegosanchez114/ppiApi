using System.ComponentModel.DataAnnotations;

namespace UPIT.Domain.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        [Required]
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
        public DateTime RoleCreated { get; set; }
        public DateTime RoleUpdated { get; set; }
    }
}
