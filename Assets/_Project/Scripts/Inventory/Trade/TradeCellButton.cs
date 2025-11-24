using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TradeCellButton : GameButton
{
    public Item Item { get; set; }
    public Item ItemForTrade { get; set; }

    [SerializeField] private Image itemImage;
    [SerializeField] private Image itemForTradeImage;

    private IInventory inventory;
    [Inject] public void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }
    
    public override void Start()
    {
        base.Start();
        itemImage.sprite = Item.sprite;
        itemForTradeImage.sprite = ItemForTrade.sprite;
    }

    public override void Action()
    {
        base.Action();
        if (!isMouseOn) return;

        bool hasItem = false;
        foreach (var item in inventory.Items)
        {
            if (item == ItemForTrade) hasItem = true;
        }
        
        if (!inventory.Items.Contains(ItemForTrade) || !InventoryUI.HasFreeSpaceFor(Item.type) & !hasItem) return;
        
        inventory.RemoveItem(ItemForTrade);
        inventory.AddItem(Item);
    }
}
