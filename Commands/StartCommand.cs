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
            var chatId = Update.Message.Chat.Id;
            var keyboardOpts = new InlineKeyboardMarkup();
            keyboardOpts.AddButton(InlineKeyboardButton.WithUrl("▶️ Dono", "https://t.me/adorabat"))
                .AddNewRow().AddButton(text: "⚒️ Comandos", callbackData: "comandos");
            await BotClient.SendMessage(chatId, "*Oi! Eu sou a Adorabat! Vamos ser heróis juntos, igualzinho ao Mao Mao! YAAAY! 💙✨ O que você quer fazer agora?!*", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: keyboardOpts );
        }
    }
}