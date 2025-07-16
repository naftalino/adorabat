using bot.Database;
using bot.Models;
using Microsoft.EntityFrameworkCore;

namespace bot.Services
{
    public class UserService
    {
        private readonly AppDbContext _user;

        public UserService(AppDbContext user)
        {
            _user = user;
        }

        public async Task<User?> GetUserById(long userId)
        {
            return await _user.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _user.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> CreateUser(User user)
        {
            _user.Users.Add(user);
            await _user.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            _user.Users.Update(user);
            await _user.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUser(long userId)
        {
            var user = await GetUserById(userId);
            if (user != null)
            {
                _user.Users.Remove(user);
                await _user.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _user.Users.ToListAsync();
        }

        public async Task<bool> UserExists(long userId)
        {
            return await _user.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<List<User>> Ranking(int top = 10)
        {
            return await _user.Users
                .OrderByDescending(u => u.TotalSpent) // Assuming PurchaseCount is a property tracking purchases
                .Take(top)
                .ToListAsync();
        }

        public async Task<List<User>> SearchUsersByProductsBought(Product product)
        {
            return await _user.Users
                .Where(u => u.Orders.Any(p => p.ProductId == product.Id))
                .ToListAsync();
        }
    }
}
