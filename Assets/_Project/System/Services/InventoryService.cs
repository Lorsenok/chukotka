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
    private const string SAVE_KEY = "InventoryItems";
    private List<Item> items = new List<Item>();

    public List<Item> Items
    {
        get => items;
        set
        {
            items = value;
        }
    }

    public Action OnItemsChanged { get; set; }

    public InventoryService()
    {
        LoadItems();
        OnItemsChanged += SaveItems;
    }

    private void SaveItems()
    {
        ItemList itemList = new ItemList { Items = items };
        string json = JsonUtility.ToJson(itemList);
        GameSaver.Save(SAVE_KEY, json);
    }

    private void LoadItems()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = (string)GameSaver.Load(SAVE_KEY, typeof(string));
            ItemList itemList = JsonUtility.FromJson<ItemList>(json);
            items = itemList.Items ?? new List<Item>();
        }
        else
        {
            items = new List<Item>();
        }
    }

    [System.Serializable]
    private class ItemList
    {
        public List<Item> Items;
    }
}