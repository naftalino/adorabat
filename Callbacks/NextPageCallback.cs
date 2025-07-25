using bot.Services;
using bot.Tools;
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
            int page = int.Parse(newPage[1]);

            var paged = await _shop.GetPagedProducts(page: page);

            string msg = $"🛍️ Produtos disponíveis:\n";

            foreach (var product in paged.Items)
            {
                msg += $"\n<blockquote><a href=\"https://t.me/adorabatbot?start={WebUtility.UrlEncode(product.Id.ToString())}\">{WebUtility.HtmlEncode(product.Name)}</a></blockquote>";
            }

            var keyboard = PaginationKeyboardBuilder.BuildPagination(
                paged.CurrentPage,
                paged.TotalPages,
                callbackPrefix: "NextPage"
            );

            await Bot.EditMessageText(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId, msg, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: keyboard);
            await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id);
        }
    }
}
