using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Commands
{
    public abstract class BaseCommand
    {
        protected ITelegramBotClient BotClient;
        protected Update Update;

        public BaseCommand(ITelegramBotClient botClient, Update update)
        {
            BotClient = botClient;
            Update = update;
        }

        // Execução principal do comando
        public async Task ExecuteAsync()
        {
            if (!await UserHasPermission()) return;
            if (await IsSpam()) return;

            await RunCommand(); // Implementado na classe filha
        }

        protected abstract Task RunCommand();
        protected virtual async Task<bool> UserHasPermission()
        {
            // Implementar verificação de permissão
            return true;
        }
        protected virtual async Task<bool> IsSpam()
        {
            // Implementar controle de spam
            return false;
        }
    }
}