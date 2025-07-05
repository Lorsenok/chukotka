using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    List<Item> Items { get; set; }
}

public class InventoryService : IInventory
{
    public List<Item> Items { get; set; } = new();
}