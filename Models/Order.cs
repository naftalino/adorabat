namespace bot.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }

        // relacionamento
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; } = false;
    }
}
