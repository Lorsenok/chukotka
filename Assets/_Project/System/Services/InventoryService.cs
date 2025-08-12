using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    List<Item> Items { get; set; }
    Action OnItemsChanged { get; set; }
}

public class InventoryService : IInventory
{
    private List<Item> items;

    public List<Item> Items
    {
        get { return items; }
        set
        {
            items = value;
            OnItemsChanged?.Invoke();
        }
    }
    public Action OnItemsChanged { get; set; }
}