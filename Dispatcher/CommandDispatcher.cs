using bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Dispatcher
{
    public class CommandDispatcher
    {
        private ITelegramBotClient _botClient;

        public CommandDispatcher(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task DispatchAsync(Update update)
        {
            var message = update.Message?.Text?.Split(' ')[0];
            var call = update.CallbackQuery.Data;

            Console.WriteLine(call);

            BaseCommand? command = message switch
            {
                "/start" => new StartCommand(_botClient, update),
                "/help" => new HelpCommand(_botClient, update),
                _ => null
            };

            if (command != null)
                await command.ExecuteAsync();
        }
    }

}
