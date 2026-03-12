using System.Collections.Generic;
using InventoryDB.Models.Database;

namespace InventoryDB.Repository.Inventories
{
    public interface IInventoryRepository
    {
        List<InventoryItem> GetItemsByUser(int userId, string search, int page, int pageSize);
        int GetTotalItems(int userId, string search);
        InventoryItem GetItemById(int id);
        void AddItem(InventoryItem item);
        void UpdateItem(InventoryItem item);
        void DeleteItem(int id);
    }
}