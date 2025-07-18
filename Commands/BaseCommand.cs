using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Commands
{
    public abstract class BaseCommand : IDisposable
    {
        protected ITelegramBotClient BotClient;
        protected Update Update;

        private IServiceScope? _scope;

        public BaseCommand(ITelegramBotClient botClient, Update update)
        {
            BotClient = botClient;
            Update = update;
        }

        public void SetScope(IServiceScope scope)
        {
            _scope = scope;
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }

        public async Task ExecuteAsync()
        {
            if (!await UserHasPermission()) return;
            if (await IsSpam()) return;
            await RunCommand();
        }

        protected abstract Task RunCommand();
        protected virtual Task<bool> UserHasPermission() => Task.FromResult(true);
        protected virtual Task<bool> IsSpam() => Task.FromResult(false);
    }

}
