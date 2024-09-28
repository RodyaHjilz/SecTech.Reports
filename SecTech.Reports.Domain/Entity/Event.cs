using System.ComponentModel.DataAnnotations;

namespace SecTech.Reports.Domain.Entity
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
