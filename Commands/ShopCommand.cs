using bot.Attributes;
using bot.Models;
using bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Commands
{
    [OnlyPrivate]
    [BotCommand("shop", "Acesso ao itens, serviços, etc.")]
    public class ShopCommand : BaseCommand
    {
        private readonly ShopService _shop;

        public ShopCommand(ITelegramBotClient botClient, Update update, ShopService shop) : base(botClient, update)
        {
            _shop = shop;
        }

        protected override async Task RunCommand()
        {
            string msg = "🛍 Produtos da lojinha:\n";
            List<Product> products = await _shop.GetAllProducts();
            if (products.Count == 0)
            {
                msg = "⚠️ Nenhum produto cadastrado, volte em breve.";
            }
            else
            {
                foreach (var product in products)
                {
                    var emoji = GetEmojiByCategory(product.Category);
                    var link = $"https://t.me/adorabatbot?start={product.Id}";
                    msg += $"\n{emoji} <a href=\"{link}\">{product.Name}</a> — R${product.Price}";
                }
            }
            await BotClient.SendChatAction(Update.Message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
            await BotClient.SendMessage(Update.Message.Chat.Id, msg, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        private string GetEmojiByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return "❓";

            var cat = category.ToLower();

            return cat.ToLower() switch
            {
                "web" or "internet" or "software" or "digital" => "🌐",
                "serviço" or "servicos" or "service" or "trabalho" or "worker" => "🛠️",
                _ => "📦"
            };
        }
    }
}
