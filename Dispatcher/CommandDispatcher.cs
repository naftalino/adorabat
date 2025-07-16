using bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message?.Text?.Split(' ')[0];

                BaseCommand? command = message switch
                {
                    "/start" => new StartCommand(_botClient, update),
                    "/help" => new HelpCommand(_botClient, update),
                    _ => null
                };

                if (command != null)
                    await command.ExecuteAsync();
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(update.CallbackQuery);
            }
        }

        private async Task HandleCallbackQuery(CallbackQuery callback)
        {
            var chatId = callback?.Message.Chat.Id;
            var messageId = callback.Message.Id;
            var data = callback?.Data;

            switch (data)
            {
                case "comandos":
                    await _botClient.EditMessageText(chatId, messageId, "*Lista de comandos*\r\n\r\n/start - Inicia o bot\r\n/help - Uma pequena descrição\r\n\r\n\r\n_❗️Mais em breve..._", parseMode: ParseMode.Markdown);
                    break;

                default:
                    await _botClient.AnswerCallbackQuery(callback.Id, "Comando não reconhecido.", showAlert: true);
                    break;
            }

            // Responde o callback (remove loading do botão no Telegram)
            await _botClient.AnswerCallbackQuery(callback.Id);
        }
    }
}