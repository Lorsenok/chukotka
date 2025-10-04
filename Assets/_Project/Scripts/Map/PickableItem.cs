using UnityEngine;
using Zenject;

public class PickableItem : DialogueTriggerMessage
{
    [SerializeField] private Item item;

    private IInventory inventory;
    [Inject] private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }

    public override void Action()
    {
        base.Action();
        if (!isPlayerOn || !InventoryUI.HasFreeSpaceFor(item.type) & !inventory.Items.Contains(item)) return;
        inventory.Items.Add(item);
        inventory.OnItemsChanged?.Invoke();
        Destroy(gameObject);
    }
}
