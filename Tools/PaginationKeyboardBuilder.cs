using Telegram.Bot.Types.ReplyMarkups;

namespace bot.Tools
{
    public static class PaginationKeyboardBuilder
    {
        public static InlineKeyboardMarkup BuildPagination(int currentPage, int totalPages, string callbackPrefix)
        {
            var buttons = new List<InlineKeyboardButton>();

            if (currentPage > 1)
            {
                buttons.Add(InlineKeyboardButton.WithCallbackData("⬅️", $"{callbackPrefix}:{currentPage - 1}"));
            }

            buttons.Add(InlineKeyboardButton.WithCallbackData($"📄 {currentPage}/{totalPages}", "noop"));

            if (currentPage < totalPages)
            {
                buttons.Add(InlineKeyboardButton.WithCallbackData("➡️", $"{callbackPrefix}:{currentPage + 1}"));
            }

            return new InlineKeyboardMarkup(new[] { buttons });
        }
    }
}
