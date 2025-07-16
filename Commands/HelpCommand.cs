using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Commands
{
    public class HelpCommand : BaseCommand
    {
        public HelpCommand(ITelegramBotClient botClient, Update update) : base(botClient, update) { }

        protected override async Task RunCommand()
        {
            await BotClient.SendMessage(Update.Message.Chat.Id, 
                "Em andamento...", ParseMode.Html);
        }
    }
}
