using System.ComponentModel.DataAnnotations;

namespace SecTech.Reports.Domain.Entity
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        [Required]
        public Guid EventId { get; set; }
        public string? EventName { get; set; }
        public DateTime CheckInTime { get; set; }
        public bool IsOnTime { get; set; }

    }
}
