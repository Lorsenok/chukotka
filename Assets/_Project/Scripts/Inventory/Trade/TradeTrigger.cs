using UnityEngine;
using Zenject;

public class TradeTrigger : DialogueTriggerMessage
{
    [Header("Trade")]
    [SerializeField] private Trade trade;
    [SerializeField] private TradeItem[] items;

    private IInventory inventory;
    [Inject] private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }

    public override void Action()
    {
        base.Action();
        if (isPlayerOn) trade.Open(items);
    }

    public override void OnLeave(GameObject obj)
    {
        base.OnLeave(obj);
        if (target == obj) trade.Close();
    }
}
