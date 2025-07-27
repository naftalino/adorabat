using bot.Attributes;
using bot.Models;
using bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Commands
{
    [OnlyPrivate]
    [OnlyAdmins]
    [BotCommand("addproduct", "Adicionar produto à loja.")]

    public class AddProductCommand : BaseCommand
    {
        private readonly ShopService _shop;

        public AddProductCommand(ITelegramBotClient botClient, Update update, ShopService shop) : base(botClient, update)
        {
            _shop = shop;
        }

        protected override async Task RunCommand()
        {
            var msg = Update.Message.Text.Split('|');
            if (msg.Count() < 2)
            {
                await BotClient.SendMessage(Update.Message.Chat.Id, "⚠️ Você precisa informar os dados do produto. Use este exemplo:\n\n<pre>Nome|descrição|preço|imagem|categoria</pre>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                return;
            }
            Product newProduct = new() { Name = msg[0].Split()[1], Description = msg[1], Price = decimal.Parse(msg[2]), ImageUrl = msg[3], Category = msg[4] };
            await _shop.AddProduct(newProduct);
            await BotClient.SendMessage(Update.Message.Chat.Id, "O Produto foi adicionado.");
        }
    }
}
