using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Reports.Domain.Interfaces.Services
{
    public interface IAttendanceService
    {
        public Task<string> GetAllAttendancesCsv();
    }
}
