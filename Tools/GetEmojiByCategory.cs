namespace bot.Tools
{
    public static class GetEmojiByCategory
    {
        public static string GetEmoji(string category)
        {
            string emoji;
            switch (category.ToLower())
            {
                case "web":
                    emoji = "🌐";
                    break;
                case "serviço" or "servico":
                    emoji = "👷‍♂️";
                    break;
                case "automation":
                    emoji = "🤖";
                    break;
                default:
                    emoji = "☐";
                    break;
            }

            return emoji;
        }
    }
}
