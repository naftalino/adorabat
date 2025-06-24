using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            stopwatch.Stop();
            var pingMs = stopwatch.ElapsedMilliseconds;
            return Ok(new { message = "Pong!", ping = pingMs });
        }
    }
}
