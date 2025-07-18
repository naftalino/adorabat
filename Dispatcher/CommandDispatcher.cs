using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Dispatcher
{
    public class CommandDispatcher
    {
        private readonly CommandFactory _factory;

        public CommandDispatcher(CommandFactory factory)
        {
            _factory = factory;
        }

        public async Task DispatchAsync(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message?.Text?.Split(' ')[0];

                Console.WriteLine(message);
                if (string.IsNullOrWhiteSpace(message) || !message.StartsWith('/'))
                    return; // ignora se não for comando

                var command = _factory.CreateCommand(message, update);

                if (command != null)
                {
                    using (command)
                        await command.ExecuteAsync();
                }
            }

            // etc...
        }
    }
}
