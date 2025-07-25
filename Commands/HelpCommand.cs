using bot.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Commands
{
    [BotCommand("help", "Exibe a ajuda para uso básico.")]
    public class HelpCommand : BaseCommand
    {
        public HelpCommand(ITelegramBotClient botClient, Update update) : base(botClient, update) { }

        protected override async Task RunCommand()
        {
            await BotClient.SendChatAction(Update.Message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
            string mensagem = """
                
<b>⚙️ Comandos disponíveis:</b>

/help - <i>Exibe esta mensagem de ajuda</i>
/ranking - <i>Exibe o ranking dos usuários</i>
/redeem - <i>Resgata um código de resgate (geralmente enviado pelo dono do bot)</i>
/discount - <i>Exibe itens com desconto</i>
""";
            await BotClient.SendMessage(Update.Message.Chat.Id, mensagem, ParseMode.Html);
        }
    }
}
