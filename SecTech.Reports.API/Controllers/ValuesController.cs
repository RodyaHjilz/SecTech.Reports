using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Reports.Domain.Entity;
using SecTech.Reports.Domain.Interfaces.Repository;

namespace SecTech.Reports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IBaseRepository<Attendance> baseRepository;

        public ValuesController(IBaseRepository<Attendance> baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        [HttpGet]
        public ActionResult isOkey()
        {
            return Ok();
        }


    }
}
