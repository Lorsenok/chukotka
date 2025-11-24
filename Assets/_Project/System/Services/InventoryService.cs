using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    public event Action OnItemsChanged;
    IReadOnlyList<Item> Items { get; }
    void AddItem(Item item);
    void AddRangeItems(List<Item> items);
    void RemoveItem(Item item);
}

public class InventoryService : IInventory
{
    private const string SAVE_KEY = "InventoryItems";
    private List<Item> _items = new List<Item>();

    public event Action OnItemsChanged;
    
    public IReadOnlyList<Item> Items => _items;
    
    public InventoryService()
    {
        LoadItems();
        OnItemsChanged += SaveItems;
    }

    public void AddItem(Item item)
    {
        _items.Add(item);
        OnItemsChanged?.Invoke();
    }

    public void AddRangeItems(List<Item> items)
    {
        _items.AddRange(items);
        OnItemsChanged?.Invoke(); 
    }

    public void RemoveItem(Item item)
    {
        _items.Remove(item);
        OnItemsChanged?.Invoke();
    }

    private void SaveItems()
    {
        ItemList itemList = new ItemList { Items = _items };
        string json = JsonUtility.ToJson(itemList);
        GameSaver.Save(SAVE_KEY, json);
    }

    private void LoadItems()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = (string)GameSaver.Load(SAVE_KEY, typeof(string));
            ItemList itemList = JsonUtility.FromJson<ItemList>(json);
            _items = itemList.Items ?? new List<Item>();
        }
        else
        {
            _items = new List<Item>();
        }
    }

    [System.Serializable]
    private class ItemList
    {
        public List<Item> Items;
    }
}