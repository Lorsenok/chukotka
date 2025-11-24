using UnityEngine;
using Zenject;

public class InventoryItemTrigger : Trigger
{
    [Tooltip("Что положить в инвентарь")] [SerializeField]
    private Item[] _itemsToPutDown;

    [Tooltip("Что достать из инвентаря")] [SerializeField]
    private Item[] _itemsToPickUp;
    
    [SerializeField] private bool _isUsedOnStart;

    private IInventory _inventory;

    [Inject]
    private void Init(IInventory inventory)
    {
        _inventory = inventory;
    }
    
    private void Start()
    {
        if (_isUsedOnStart) 
            Action();
    }

    public override void Action()
    {
        foreach (Item item in _itemsToPutDown)
        {
            _inventory.AddItem(item);
        }
        
        foreach (Item item in _itemsToPickUp)
        {
            _inventory.RemoveItem(item);
        }
        
        base.Action();
    }
}