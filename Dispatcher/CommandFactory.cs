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

            var scope = _provider.CreateScope();
            var services = scope.ServiceProvider;

            var command = (BaseCommand)ActivatorUtilities.CreateInstance(
                services, type, services.GetRequiredService<ITelegramBotClient>(), update);

            command.SetScope(scope);

            return command;
        }
    }

}
