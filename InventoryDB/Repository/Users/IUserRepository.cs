using InventoryDB.Models.Database;

namespace InventoryDB.Repository.Users
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        bool UsernameExists(string username);
        void AddUser(User user);
        User ValidateUser(string username, string passwordHash);
    }
}