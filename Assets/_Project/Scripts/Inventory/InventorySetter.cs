using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventorySetter : MonoBehaviour
{
    [SerializeField] private List<Item> items;

    private IInventory inventory;

    [Inject]
    private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }

    private void OnEnable()
    {
        if (enabled)
        {
            inventory.Items = items;
            inventory.OnItemsChanged?.Invoke();
        }
    }
}