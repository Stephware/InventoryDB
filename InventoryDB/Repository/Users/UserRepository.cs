using System.Linq;
using InventoryDB.Models.Database;

namespace InventoryDB.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly InventoryDbContext _context;

        public UserRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public User GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public bool UsernameExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User ValidateUser(string username, string passwordHash)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);
        }
    }
}