using Microsoft.AspNetCore.Mvc;

namespace bot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceivePayment([FromBody] object paymentData)
        {
            Console.WriteLine(paymentData);
            return Ok();
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { response = "Ok" });
        }
    }
}
