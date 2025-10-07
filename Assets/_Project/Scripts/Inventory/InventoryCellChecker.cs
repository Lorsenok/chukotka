using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryCellChecker : MonoBehaviour
{
    public Item item;
    [SerializeField] private List<InventoryCell> cells;

    private IInventory inventory;
    [Inject]
    private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }
    
    private void Start()
    {
        if (cells.Count != 0) return;
        CellUser[] users = FindObjectsByType<CellUser>(FindObjectsSortMode.None);
        foreach (CellUser cellUser in users)
        {
            cells.Add(cellUser.GetComponent<InventoryCell>());
        }
    }

    public bool HasItem { get; private set; } = false;
    private void Update()
    {
        bool onceEntered = false;
        foreach (InventoryCell cell in cells)
        {
            if (cell.ItemObj == null) continue;
            if (cell.ItemObj.Item == item && !HasItem)
            {
                HasItem = true;
                onceEntered = true;
            }
        }

        if (!onceEntered && HasItem)
        {
            HasItem = false;
        }
    }
}
