namespace bot.Models
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().ToLower();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0.0;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsAvailable { get; set; } = true;
        public string Category { get; set; } = string.Empty;
        public List<Order> Orders { get; set; } = new();
    }
}
