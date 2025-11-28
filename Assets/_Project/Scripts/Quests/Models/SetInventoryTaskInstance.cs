public class SetInventoryTaskInstance : TaskInstance
{
    private readonly InventoryItemTrigger _inventoryItemTrigger;
    private bool _done;
    
    public SetInventoryTaskInstance(InventoryItemTrigger inventoryItemTrigger)
    {
        _inventoryItemTrigger = inventoryItemTrigger;
    }

    public override void Start()
    {
        _inventoryItemTrigger.Action();
        _done = true;
        Complete();
    }

    public override void Update() { }

    public override void Stop() { }

    public override bool IsCompleted => _done;
}
