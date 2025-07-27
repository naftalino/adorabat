using bot.Attributes;
using bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot.Callbacks
{
    [AntiSpam]
    public class MyProfileCallback : BaseCallbackHandler
    {
        private readonly UserService _user;

        public MyProfileCallback(ITelegramBotClient bot, Update update, UserService user) : base(bot, update)
        {
            _user = user;
        }

        public override async Task ExecuteAsync()
        {
            var keyboard = new InlineKeyboardMarkup();
            keyboard.AddButton("◀️ Voltar", callbackData: "BackToMenu:main");

            var usuario = await _user.GetUserById(Update.CallbackQuery.Message.Chat.Id);
            string profile;
            if (usuario == null)
            {
                profile = "❌ Nada encontrado no banco de dados.";
            }
            else
            {
                profile = $"""
👤 Seu perfil

🆔 ID: {usuario.Id}
👤 Username: @{(string.IsNullOrEmpty(usuario.Username) ? "N/A" : usuario.Username)}

💰 Total Gasto: R${usuario.TotalSpent}
🛒 Pedidos: {(usuario.Orders != null ? usuario.Orders.Count : "[0]")}
📅 Registrado em: {usuario.CreatedAt:dd/MM/yy}

🔔 Notifications: {(usuario.WantNotifications ? "✅" : "❌")}
{(usuario.IsAdmin ? "🛡️ Admin: Sim" : "")}
""";
            }
            await Bot.EditMessageText(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.Id, profile, replyMarkup: keyboard);
            await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id);
        }
    }
}
