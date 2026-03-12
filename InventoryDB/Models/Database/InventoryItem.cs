using System;
using System.Collections.Generic;

namespace InventoryDB.Models.Database;

public partial class InventoryItem
{
    public int Id { get; set; }

    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime DateAdded { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
