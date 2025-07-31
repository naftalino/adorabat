using bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Services
{
    public class ProcessOrderService
    {
        private readonly ITelegramBotClient _bot;

        public ProcessOrderService(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        public async Task ProcessTypeOrder(Order order)
        {
            switch (order.Type.ToLower())
            {
                case "link":
                    await SendLinkToUser(order.UserId, order.Content);
                    break;
                case "file":
                    await SendFileToUser(order.UserId, order.Content);
                    break;
                default:
                    throw new ArgumentException("Tipo de pedido desconhecido.");
            }
        }

        public async Task SendLinkToUser(long chatId, string link)
        {
            await _bot.SendMessage(chatId, link);
        }

        public async Task SendFileToUser(long chatId, string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("Erro: O arquivo não existe.", filePath);
            }
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await _bot.SendDocument(chatId, new InputFileStream(stream), "Aqui está seu pedido! Obrigado pela preferência!!");
            }
        }
    }
}
