using bot.Database;
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
    }
}
