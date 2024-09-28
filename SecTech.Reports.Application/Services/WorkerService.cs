using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using SecTech.Reports.Domain.Entity;
using SecTech.Reports.Domain.Interfaces.Repository;
using SecTech.Reports.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            //Event evnt;
            //User user;
            //Attendance attendance;
            var attendance = JsonSerializer.Deserialize<Attendance>(message);
            await _attendanceRepository.CreateAsync(attendance);
            _logger.LogInformation("Attendance saved to database: {attendance}", attendance);
           // SerializeMessage(message, out evnt, out user, out attendance);
            return true;
        }

        //private void SerializeMessage(object message, out Event evnt, out User usr, out Attendance atndnc)
        //{
            
        //}

    }
}
