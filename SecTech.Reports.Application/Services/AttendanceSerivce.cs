using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SecTech.Reports.Domain.Entity;
using SecTech.Reports.Domain.Interfaces.Repository;
using SecTech.Reports.Domain.Interfaces.Services;
using System.Text;

namespace SecTech.Reports.Application.Services
{
    public class AttendanceSerivce : IAttendanceService
    {
        private readonly IBaseRepository<Attendance> _attendanceRepository;
        private readonly ILogger<AttendanceSerivce> _logger;
        public AttendanceSerivce(IBaseRepository<Attendance> attendanceRepository, ILogger<AttendanceSerivce> logger)
        {
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }

        public async Task<string> GetAllAttendancesCsv()
        {
            var attendances = await _attendanceRepository.GetAll().Select(x => new {x.UserName, x.EventName, x.CheckInTime}).AsNoTracking().ToListAsync();
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("UserName;EventName;CheckInTime;"); // Заголовок CSV

            foreach (var attendance in attendances)
            {
                csvBuilder.AppendLine($"{attendance.UserName};{attendance.EventName};{attendance.CheckInTime}");
            }

            return csvBuilder.ToString();
        }




    }
}
