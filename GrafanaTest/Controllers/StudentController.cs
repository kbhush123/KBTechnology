using Microsoft.AspNetCore.Mvc;

namespace GrafanaTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetStudent")]
        public IEnumerable<Student> Get()
        {
            List<Student> students = new List<Student>()
            {
                new Student { Name = "Kunal Bhushan" },
                new Student { Name = "Neel Beniwal" },
                new Student { Name = "Chris Love"}
            };
            return students;
        }
    }
}
