using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bot.Dispatcher;
using Telegram.Bot.Types;

namespace bot.Controllers
{
    [ApiController]
    [Route("api/update")]
    public class WebhookController : ControllerBase
    {
        private readonly CommandDispatcher _dispatcher;

        public WebhookController(CommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await _dispatcher.DispatchAsync(update);
            return Ok();
        }
    }

}
