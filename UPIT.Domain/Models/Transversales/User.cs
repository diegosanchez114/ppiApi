using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UPIT.Domain.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        [Required]
        public string UserEmail { get; set; } = string.Empty;
        [Required]
        public string UserIdentificationNumber { get; set; } = string.Empty;
        [Required]
        public string UserDocumentType { get; set; } = string.Empty;
        [Required]
        public string UserFirstName { get; set; } = string.Empty;
        [Required]
        public string UserLastName { get; set; } = string.Empty;
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public Guid EntityId { get; set; }
        [Required]
        public string UserPhoneNumber { get; set; } = string.Empty;
        [Required]
        public string UserPosition { get; set; } = string.Empty;
        public bool UserState { get; set; }
        [Required]
        public string UserPassword {  get; set; } = string.Empty;
        public DateTime UserCreated { get; set; }
        public DateTime UserUpdated { get; set; }
    }
}
