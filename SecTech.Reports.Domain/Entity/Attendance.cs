using System.ComponentModel.DataAnnotations;

namespace SecTech.Reports.Domain.Entity
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public DateTime CheckInTime { get; set; }
        public bool IsOnTime { get; set; }

    }
}
