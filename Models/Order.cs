namespace bot.Models
{
    public class Order
    {
        public long PaymentId { get; set; }
        public PaymentInfo Payment { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; } = false;
    }
}
