using bot.Services;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot.Callbacks
{
    public class ProductsCallback : BaseCallbackHandler
    {
        private readonly ShopService _shop;

        public ProductsCallback(ITelegramBotClient bot, Update update, ShopService shop) : base(bot, update)
        {
            _shop = shop;
        }

        public async override Task ExecuteAsync()
        {
            var products = await _shop.GetPagedProducts();
            var keyboard = new InlineKeyboardMarkup();
            keyboard.AddButton("⬅️", $"BackPage:{products.CurrentPage - 1}");
            keyboard.AddButton("➡️", $"NextPage:{products.CurrentPage + 1}");

            string msg = $"🛍️ Produtos disponíveis:\n\nPágina [{products.CurrentPage}/{products.TotalPages}]\n";

            foreach (var product in products.Items)
            {
                msg += $"\n<blockquote><a href=\"https://t.me/adorabatbot?start={WebUtility.UrlEncode(product.Id.ToString())}\">{WebUtility.HtmlEncode(product.Name)}</a></blockquote>";
            }

            await Bot.EditMessageText(Update.CallbackQuery.Message.Chat.Id,
                                      Update.CallbackQuery.Message.Id,
                                      msg,
                                      replyMarkup: keyboard,
                                      parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                                      );
            await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id);
        }
    }
}
