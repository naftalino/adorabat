using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Callbacks
{
    public abstract class BaseCallbackHandler : IDisposable
    {
        protected readonly ITelegramBotClient Bot;
        protected readonly Update Update;

        public BaseCallbackHandler(ITelegramBotClient bot, Update update)
        {
            Bot = bot;
            Update = update;
        }

        public abstract Task ExecuteAsync();
        public virtual void Dispose() { }
    }

}
