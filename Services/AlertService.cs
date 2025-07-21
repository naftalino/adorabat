using bot.Database;
using bot.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace bot.Services
{
    public class AlertService
    {
        private readonly AppDbContext _alert;
        private readonly ITelegramBotClient _botClient;

        public AlertService(ITelegramBotClient botClient, AppDbContext alert)
        {
            _alert = alert;
            _botClient = botClient;
        }

        public async Task AlertAllUsers(string message)
        {
            var users = await _alert.Users.ToListAsync();
            foreach (var user in users)
            {
                await _botClient.SendMessage(user.Id, message);
            }
        }

        public async Task AlertAboutNewProduct(string message, Product product)
        {
            var users = await _alert.Users.Where(u => u.WantNotifications == true).ToListAsync();

            foreach (var user in users)
            {
                string link = $"<a href=\"https://t.me/adorabatbot?start={product.Id}\">{product.Name}</a>";
                var msg = $"🆕 Novo produto na loja!\n🛍 {link}";
                await _botClient.SendMessage(user.Id, msg, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }
        }
    }
}
