using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using InventoryDB.Models.Database;
using InventoryDB.Repository.Inventories;

namespace InventoryDB.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        //Views Inventory Index
        public IActionResult Index(string search, int page = 1)
        {
        
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int pageSize = 10;
            var items = _inventoryRepository.GetItemsByUser(userId.Value, search, page, pageSize);

            ViewBag.TotalItems = _inventoryRepository.GetTotalItems(userId.Value, search);
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;

            return View(items);
        }

        //Inventory Create
        [HttpPost]
        public IActionResult Create(string itemName, int quantity, decimal price)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var newItem = new InventoryItem
            {
                ItemName = itemName,
                Quantity = quantity,
                Price = price,
                UserId = userId.Value
            };

            _inventoryRepository.AddItem(newItem);
            return RedirectToAction("Index");
        }

        //Inventory Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            _inventoryRepository.DeleteItem(id);
            return RedirectToAction("Index");
        }
    }
}