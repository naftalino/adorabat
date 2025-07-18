namespace bot.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BotCommandAttribute : Attribute
    {
        public string Command { get; }
        public string Description { get; }

        public BotCommandAttribute(string command, string description)
        {
            Command = command;
            Description = description;
        }
    }
}
