using System.Linq;
using UnityEngine;
using Zenject;

public class PickableItem : DialogueTriggerMessage
{
    [SerializeField] private Item item;
    [SerializeField] private string savekey;

    private IInventory inventory;
    [Inject] private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }

    public override void Start()
    {
        base.Start();
        if (string.IsNullOrEmpty(savekey)) return;
        if ((int)GameSaver.Load(savekey, typeof(int)) == 1) Destroy(gameObject);
    }

    public override void Action()
    {
        base.Action();
        if (!isPlayerOn || !InventoryUI.HasFreeSpaceFor(item.type) & !inventory.Items.Contains(item)) return;
        inventory.AddItem(item);
        if (!string.IsNullOrEmpty(savekey)) GameSaver.Save(savekey, 1);
        Destroy(gameObject);
    }
}
