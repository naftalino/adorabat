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
        private readonly UserService _user;

        public StartCommand(ITelegramBotClient botClient, Update update, ShopService shop, UserService user)
            : base(botClient, update)
        {
            _shop = shop;
            _user = user;
        }

        protected override async Task RunCommand()
        {
            var messageText = Update.Message.Text ?? string.Empty;
            var parts = messageText.Split(' ', 2);
            string? param = parts.Length > 1 ? parts[1] : null;

            if (await _user.UserExist(Update.Message.Chat.Id) == false)
            {
                var newUser = new Models.User { Id = Update.Message.Chat.Id, Username = Update.Message.Chat.Username };
                await _user.CreateUser(newUser);
            }

            if (Guid.TryParse(param, out Guid productId))
            {
                var product = await _shop.GetProductById(productId);
                if (product != null)
                {
                    var keyboard = new InlineKeyboardMarkup();
                    keyboard.AddButton(text: "💸 Comprar agora", callbackData: $"Buy:{productId}");
                    string message = $"""
<b>🛍️ Nome:</b> {product.Name}
{(product.IsAvailable ? "✅ <b>Status:</b> <i>Disponível</i>" : "❌ <b>Status:</b> <i>Indisponível</i>")}

📦 <b>Categoria:</b> {(product.Category.ToLower() == "web" ? "🌐 Web" : "🛠️ Serviço")}
💸 <b>Preço:</b> <code>R$ {product.Price:F2}</code>

📝 <b>Descrição:</b>
{product.Description}

📅 <b>Adicionado em:</b> {product.CreatedAt:dd/MM/yyyy - HH:mm}
""";
                    await BotClient.SendMessage(
                        Update.Message.Chat.Id,
                        message,
                        replyMarkup: keyboard,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    return;
                }
                else
                {
                    await BotClient.SendMessage(Update.Message.Chat.Id, $"❌ Produto não encontrado. {productId}");
                    return;
                }
            }

            await BotClient.SendChatAction(Update.Message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
            string mensagem = """
👋 Olá, seja bem-vindo à <b>Adorabat Shop</b>!

🛠️ Aqui você encontra bots, sistemas e ferramentas digitais feitas sob medida pra agilizar sua rotina.

💳 Escolha um produto, faça o pagamento e receba tudo direto por aqui no Telegram.

🔽 Use o menu abaixo ou envie /shop pra começar.
""";
            var chatId = Update.Message.Chat.Id;
            var keyboardOpts = new InlineKeyboardMarkup();
            keyboardOpts.AddButton(text: "🛍 Produtos", callbackData: "Products")
            .AddButton(text: "📦 Meus Pedidos", callbackData: "MyOrders")
                        .AddNewRow()
            .AddButton(text: "📊 Meu Perfil", callbackData: "MyProfile")
            .AddButton(InlineKeyboardButton.WithUrl(text: "💬 Fale com Suporte", "https://t.me/adorabat"))
            .AddNewRow()
            .AddButton(InlineKeyboardButton.WithUrl(text: "📩 Encomendar Software/Bot", "https://t.me/adorabat"));
            await BotClient.SendMessage(chatId, mensagem, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: keyboardOpts);
        }
    }
}
