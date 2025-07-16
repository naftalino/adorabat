using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot.Commands
{
    public class StartCommand : BaseCommand
    {

        public StartCommand(ITelegramBotClient botClient, Update update)
            : base(botClient, update) { }

        protected override async Task RunCommand()
        {
            string mensagem = """
👋 Olá, seja bem-vindo à <b>Adorabat Shop</b>!

🛠️ Aqui você encontra bots, sistemas e ferramentas digitais feitas sob medida pra agilizar sua rotina.

💳 Escolha um produto, faça o pagamento e receba tudo direto por aqui no Telegram.

🔽 Use o menu abaixo ou envie /loja pra começar.

Qualquer dúvida, só <a href="t.me/adorabat">chamar!</a>
""";
            var chatId = Update.Message.Chat.Id;
            var keyboardOpts = new InlineKeyboardMarkup();
            keyboardOpts.AddButton(text: "🛍 Produtos", callbackData: "ver_produtos")
            .AddButton(text: "📦 Meus Pedidos", callbackData: "meus_pedidos")
                        .AddNewRow()
            .AddButton(text: "📊 Meu Perfil", callbackData: "meu_perfil")
            .AddButton(InlineKeyboardButton.WithUrl(text: "💬 Fale com Suporte", "https://t.me/adorabat"))
            .AddNewRow()
            .AddButton(InlineKeyboardButton.WithUrl(text: "📩 Encomendar Software/Bot", "https://t.me/adorabat"));
            await BotClient.SendMessage(chatId, mensagem, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: keyboardOpts);
        }
    }
}
