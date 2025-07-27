using bot.Attributes;
using bot.Services;
using bot.Tools;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Callbacks
{
    [AntiSpam]
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
            string msg = $"🛍️ Produtos disponíveis:\n\n";

            foreach (var product in products.Items)
            {
                msg += $"\n<blockquote><a href=\"https://t.me/adorabatbot?start={WebUtility.UrlEncode(product.Id.ToString())}\">{WebUtility.HtmlEncode(product.Name)}</a></blockquote>";
            }

            var keyboard = PaginationKeyboardBuilder.BuildPagination(
                currentPage: products.CurrentPage,
                totalPages: products.TotalPages,
                callbackPrefix: "NextPage"
            );

            await Bot.EditMessageText(
                chatId: Update.CallbackQuery.Message.Chat.Id,
                messageId: Update.CallbackQuery.Message.MessageId,
                text: msg,
                replyMarkup: keyboard,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
            );

            await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id);
        }
    }
}
