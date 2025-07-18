using bot.Attributes;
using bot.Commands;
using System.Reflection;
using Telegram.Bot.Types;

namespace bot.Dispatcher
{
    public static class BotCommandHelper
    {
        public static List<BotCommand> GetRegisteredCommands()
        {
            var commands = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(BaseCommand).IsAssignableFrom(t))
                .Select(t => t.GetCustomAttribute<BotCommandAttribute>())
                .Where(attr => attr != null)
                .Select(attr => new BotCommand
                {
                    Command = attr.Command,
                    Description = attr.Description
                })
                .ToList();

            return commands;
        }
    }
}
