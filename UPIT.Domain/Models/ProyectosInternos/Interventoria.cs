using System.ComponentModel.DataAnnotations;

namespace UPIT.Domain.Models.ProyectosInternos
{
    public class Interventoria
    {
        [Required]
        public Guid DataEntityId { get; set; }
        public Guid EntityId { get; set; }
        public int DataEntityValue { get; set; }
        public DateTime DataEntityCreated { get; set; }
        public DateTime DataEntityUpdated { get; set; }
    }
}
