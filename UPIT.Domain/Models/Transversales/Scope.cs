using System.ComponentModel.DataAnnotations;

namespace UPIT.Domain.Models
{
    public class Scope
    {
        public Guid ScopeId { get; set; }
        [Required]
        public string ScopeName { get; set; } = string.Empty;
        [Required]
        public string ScopePath { get; set; } = string.Empty;
        [Required]
        public string ScopeMethod {  get; set; } = string.Empty;
        public string ScopeDescription { get; set; } = string.Empty;
        public DateTime ScopeCreated { get; set; }
        public DateTime ScopeUpdated { get; set; }
    }
}
