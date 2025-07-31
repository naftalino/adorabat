using bot.Attributes;
using bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot.Callbacks
{
    [AntiSpam]
    public class BuyCallback : BaseCallbackHandler
    {
        private readonly ShopService _shop;
        private readonly MercadoPagoService _mp;

        public BuyCallback(ITelegramBotClient bot, Update update, ShopService shop, MercadoPagoService mp) : base(bot, update)
        {
            _shop = shop;
            _mp = mp;
        }

        public override async Task ExecuteAsync()
        {
            string productId = Update.CallbackQuery.Data.Split(':')[1];

            var product = await _shop.GetProductById(Guid.Parse(productId));
            if (product == null)
            {
                await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id, "Produto não encontrado.");
                return;
            }
            if (!product.IsAvailable)
            {
                await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id, "Produto não disponível.");
                return;
            }
            if (product.Price <= 0)
            {
                await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id, "Produto sem preço definido.");
                return;
            }

            var created = await _mp.CreatePayment(
                amount: product.Price,
                externalRerefence: product.Id
            );

            string copiCola = created.PointOfInteraction.TransactionData.QrCode;
            string linkPayment = created.PointOfInteraction.TransactionData.TicketUrl;

            var keyboard = new InlineKeyboardMarkup();
            keyboard.AddButton(InlineKeyboardButton.WithCopyText("❖ Copiar código PIX", copiCola))
            .AddNewRow()
            .AddButton(InlineKeyboardButton.WithUrl("🤝 Copiar/visualizar QR Code", linkPayment));
            await Bot.EditMessageText(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.Id, $"Seu pagamento criado! Copie o código copia e cola abaixo e pague com Pix. \n\n[⚠️ Quando o pagamento for realizado, irei receber e automaticamente realizar a verificação]", replyMarkup: keyboard);
            await Bot.AnswerCallbackQuery(Update.CallbackQuery.Id);
        }
    }
}
