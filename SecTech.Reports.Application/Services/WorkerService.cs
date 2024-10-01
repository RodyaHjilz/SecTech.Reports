using Microsoft.Extensions.Logging;
using SecTech.Reports.Domain.Entity;
using SecTech.Reports.Domain.Interfaces.Repository;
using SecTech.Reports.Domain.Interfaces.Services;
using System.Text.Json;

namespace SecTech.Reports.Application.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IBaseRepository<Attendance> _attendanceRepository;
        private readonly ILogger<WorkerService> _logger;
        public WorkerService(IBaseRepository<Attendance> attendanceRepository, ILogger<WorkerService> logger)
        {
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }
        public async Task<bool> SaveToDatabase(string message)
        {
            try
            {
                var attendance = JsonSerializer.Deserialize<Attendance>(message);
                await _attendanceRepository.CreateAsync(attendance);
                _logger.LogInformation("Attendance saved to database with id: {attendance}", attendance.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save attendance to database. Got message from Rabbit: {message}", message);
                return false;
            }
        }

    }
}
