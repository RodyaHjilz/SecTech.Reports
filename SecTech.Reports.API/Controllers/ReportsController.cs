using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Reports.Domain.Interfaces.Services;
using System.Text;

namespace SecTech.Reports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ILogger<ReportsController> _logger;
        public ReportsController(IAttendanceService attendanceService, ILogger<ReportsController> logger)
        {
            _attendanceService = attendanceService;
            _logger = logger;
        }


        [HttpGet("export")]
        public async Task<IActionResult> ExportAttendancesAsCsv()
        {
            try
            {
                var csvData = await _attendanceService.GetAllAttendancesCsv();

                // Генерация имени файла
                var fileName = $"Attendances_{DateTime.Now:yyyyMMddHHmmss}.csv";

                // Возвращаем данные как файл с правильным типом контента
                return File(Encoding.UTF8.GetBytes(csvData), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем статус 500
                _logger.LogError(ex, "Ошибка при экспорте данных посещаемости в CSV.");
                return StatusCode(500, "Произошла ошибка при экспорте данных.");
            }
        }
    }
}
