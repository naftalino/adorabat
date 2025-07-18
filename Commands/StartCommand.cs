using bot.Attributes;
using bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot.Commands
{
    [OnlyPrivate]
    [BotCommand("start", "Inicializa o bot.")]
    public class StartCommand : BaseCommand
    {
        private readonly ShopService _shop;

        public StartCommand(ITelegramBotClient botClient, Update update, ShopService shop)
            : base(botClient, update)
        {
            _shop = shop;
        }

        protected override async Task RunCommand()
        {
            var messageText = Update.Message.Text ?? string.Empty; // ex: "/start 123e4567-e89b-12d3-a456-426614174000"
            var parts = messageText.Split(' ', 2);

            string? param = parts.Length > 1 ? parts[1] : null;

            if (Guid.TryParse(param, out Guid productId))
            {
                var product = await _shop.GetProductById(productId);
                Console.WriteLine(product);
                if (product != null)
                {
                    var msg = $"🛍 <b>{product.Name}</b>\n\n" +
                              $"💰 <b>Preço:</b> R${product.Price}\n" +
                              $"📝 <b>Descrição:</b> {product.Description}";

                    await BotClient.SendMessage(
                        Update.Message.Chat.Id,
                        msg,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    return;
                }
                else
                {
                    await BotClient.SendMessage(Update.Message.Chat.Id, "❌ Produto não encontrado.");
                    return;
                }
            }

            await BotClient.SendChatAction(Update.Message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
            string mensagem = """
👋 Olá, seja bem-vindo à <b>Adorabat Shop</b>!

🛠️ Aqui você encontra bots, sistemas e ferramentas digitais feitas sob medida pra agilizar sua rotina.

💳 Escolha um produto, faça o pagamento e receba tudo direto por aqui no Telegram.

🔽 Use o menu abaixo ou envie /shop pra começar.

Qualquer dúvida, só <a href="t.me/adorabat">chamar!</a>
""";
            var chatId = Update.Message.Chat.Id;
            var keyboardOpts = new InlineKeyboardMarkup();
            keyboardOpts.AddButton(text: "🛍 Produtos", callbackData: "products")
            .AddButton(text: "📦 Meus Pedidos", callbackData: "my_orders")
                        .AddNewRow()
            .AddButton(text: "📊 Meu Perfil", callbackData: "profile")
            .AddButton(InlineKeyboardButton.WithUrl(text: "💬 Fale com Suporte", "https://t.me/adorabat"))
            .AddNewRow()
            .AddButton(InlineKeyboardButton.WithUrl(text: "📩 Encomendar Software/Bot", "https://t.me/adorabat"));
            await BotClient.SendMessage(chatId, mensagem, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: keyboardOpts);
        }
    }
}
