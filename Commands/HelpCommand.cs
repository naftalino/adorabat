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
                "<b>Olá! 👋 Sou um bot cheio de funções úteis pra facilitar sua vida aqui no Telegram. Veja abaixo os principais comandos disponíveis:</b>", ParseMode.Html);
        }
    }
}
