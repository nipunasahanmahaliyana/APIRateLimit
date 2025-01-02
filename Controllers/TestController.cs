using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRateLimit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
           
            return Ok("You are allowed to make this request.");
        }
    }
}
