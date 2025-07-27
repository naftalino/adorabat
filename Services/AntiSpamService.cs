using Microsoft.Extensions.Caching.Memory;

namespace bot.Services
{
    public class AntiSpamService
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan _spamTimeout = TimeSpan.FromSeconds(3);

        public AntiSpamService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool IsSpamming(long userId)
        {
            var key = $"spam:{userId}";

            if (_cache.TryGetValue(key, out _))
                return true;

            _cache.Set(key, true, _spamTimeout);
            return false;
        }
    }
}
