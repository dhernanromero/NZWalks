using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumbre/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET: https://localhost:portnumbre/api/students
        [HttpGet] 
        public IActionResult GetAllStudents()
        {
            string[] studentName = new string[] { "Diego", "Cristian", "Nicolas", "Jonatan" };
            return Ok(studentName);
        }
    }
}
