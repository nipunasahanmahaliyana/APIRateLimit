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
            var Text="badge";
            return Ok("You are allowed to make this request.");
        }
    }
}
