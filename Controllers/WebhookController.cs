using bot.Dispatcher;
using bot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace bot.Controllers
{
    [ApiController]
    [Route("api/update")]
    public class WebhookController : ControllerBase
    {
        private readonly CommandDispatcher _dispatcher;
        private readonly UserService _user;

        public WebhookController(CommandDispatcher dispatcher, UserService user)
        {
            _dispatcher = dispatcher;
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await _dispatcher.DispatchAsync(update, _user);
            return Ok();
        }
    }

}
