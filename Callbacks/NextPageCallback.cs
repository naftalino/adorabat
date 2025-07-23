
using bot.Services;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Callbacks
{
    public class NextPageCallback : BaseCallbackHandler
    {
        private readonly ShopService _shop;
        public NextPageCallback(ITelegramBotClient bot, Update update, ShopService shop) : base(bot, update)
        {
            _shop = shop;
        }

        public override async Task ExecuteAsync()
        {
            string[] newPage = Update.CallbackQuery.Data.Split(":");
            var nextPage = await _shop.GetPagedProducts(page: int.Parse(newPage[1]));
            string msg = $"🛍️ Produtos disponíveis:\n\nPágina [{nextPage.CurrentPage}/{nextPage.CurrentPage}]";
            foreach (var product in nextPage.Items)
            {
                msg += $"\n<blockquote><a href=\"https://t.me/adorabatbot?start={WebUtility.UrlEncode(product.Id.ToString())}\">{WebUtility.HtmlEncode(product.Name)}</a></blockquote>";
            }
            await Bot.EditMessageText(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId, msg, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id);
        }
    }
}
