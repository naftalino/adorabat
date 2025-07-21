using bot.Attributes;
using bot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Dispatcher
{
    public class CommandDispatcher
    {
        private readonly CommandFactory _factory;
        private readonly CallbackDispatcher _callbackDispatcher;

        public CommandDispatcher(CommandFactory factory, CallbackDispatcher callbackDispatcher)
        {
            _callbackDispatcher = callbackDispatcher;
            _factory = factory;
        }

        public async Task DispatchAsync(Update update, UserService user)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message?.Text?.Split(' ')[0];
                if (string.IsNullOrWhiteSpace(message) || !message.StartsWith('/'))
                    return;

                var command = _factory.CreateCommand(message, update);

                if (command != null)
                {
                    var commandType = command.GetType();

                    // ⛔ Verifica se precisa ser admin
                    if (Attribute.IsDefined(commandType, typeof(OnlyAdminsAttribute)))
                    {
                        var chatId = update.Message!.Chat.Id;
                        var userId = update.Message.From!.Id;

                        var userInfo = await user.GetUserById(userId);
                        if (!userInfo.IsAdmin)
                        {
                            return;
                        }
                    }

                    using (command)
                        await command.ExecuteAsync();
                }
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;

                if (string.IsNullOrWhiteSpace(callbackQuery?.Data))
                    return;

                var command = _callbackDispatcher.CreateHandler(callbackQuery.Data, update);
                if (command != null)
                {
                    using (command)
                        await command.ExecuteAsync();
                }
            }
        }
    }
}
