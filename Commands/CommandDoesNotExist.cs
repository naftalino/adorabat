using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Commands
{
    public class CommandDoesNotExist : BaseCommand
    {
        public CommandDoesNotExist(ITelegramBotClient botClient, Update update)
            : base(botClient, update) { }
        protected override async Task RunCommand()
        {
            string mensagem = "⚠️ Comando não reconhecido. Use /help para ver os comandos disponíveis.";
            var chatId = Update.Message.Chat.Id;
            await BotClient.SendMessage(chatId, mensagem, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
