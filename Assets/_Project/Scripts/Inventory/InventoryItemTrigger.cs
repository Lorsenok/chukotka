using UnityEngine;
using Zenject;

public class InventoryItemTrigger : Trigger
{
    [Tooltip("Что положить в инвентарь")] [SerializeField]
    private Item[] _itemsToPutDown;

    [Tooltip("Что достать из инвентаря")] [SerializeField]
    private Item[] _itemsToPickUp;

    private IInventory _inventory;

    [Inject]
    private void Init(IInventory inventory)
    {
        _inventory = inventory;
    }

    public override void Action()
    {
        foreach (Item item in _itemsToPutDown)
        {
            _inventory.Items.Add(item);
        }
        
        foreach (Item item in _itemsToPickUp)
        {
            _inventory.Items.Remove(item);
        }
        
        base.Action();
    }
}