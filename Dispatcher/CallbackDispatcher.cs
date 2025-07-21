using bot.Callbacks;
using bot.Tools;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Dispatcher
{
    public class CallbackDispatcher
    {
        private readonly IServiceProvider _provider;

        public CallbackDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        // Para criar o handler de callback, o callbackData deve ser no formato "key:value"
        // Exemplo: "start:12345" onde "start" é a chave e "12345" é o valor
        // Para o dispatcher encontrar o handler correto, o nome da classe deve ser "CallbackNameCallback"

        public BaseCallbackHandler? CreateHandler(string callbackData, Update update)
        {
            var key = callbackData.Split(':')[0];
            var typeName = $"{key.Capitalize()}Callback";

            var type = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t =>
                    typeof(BaseCallbackHandler).IsAssignableFrom(t) &&
                    t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));

            if (type == null)
                return null;

            var scope = _provider.CreateScope();
            var services = scope.ServiceProvider;

            var handler = (BaseCallbackHandler)ActivatorUtilities.CreateInstance(
                services, type, services.GetRequiredService<ITelegramBotClient>(), update);

            return handler;
        }
    }
}
