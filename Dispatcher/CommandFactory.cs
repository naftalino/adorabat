using bot.Commands;
using bot.Tools;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Dispatcher
{
    public class CommandFactory
    {
        private readonly IServiceProvider _provider;

        public CommandFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public BaseCommand? CreateCommand(string? commandText, Update update)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                return null;

            var typeName = $"{commandText.TrimStart('/').Capitalize()}Command";

            var type = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t =>
                    typeof(BaseCommand).IsAssignableFrom(t) &&
                    t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));

            if (type == null)
                return null;

            // 🧠 Cria um escopo novo e instancia o comando DENTRO dele
            var scope = _provider.CreateScope();
            var services = scope.ServiceProvider;

            // Cria a instância do comando com todos os serviços injetados
            var command = (BaseCommand)ActivatorUtilities.CreateInstance(
                services, type, services.GetRequiredService<ITelegramBotClient>(), update);

            // 🧼 Garante que o escopo será descartado depois que o comando for executado
            command.SetScope(scope);

            return command;
        }
    }

}
