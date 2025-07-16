using System.ComponentModel.DataAnnotations;

namespace bot.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public double TotalSpent { get; set; } = 0.0;
        public List<Order> Orders { get; set; } = new List<Order>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsAdmin { get; set; } = false;
        public bool WantNotifications { get; set; } = true;
    }
}
