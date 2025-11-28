public class SetInventoryTaskInstance : TaskInstance
{
    private readonly IInventory _inventory;
    private readonly Item[] _itemsToPutDown;
    private readonly Item[] _itemsToPickUp;
    
    private bool _done;
    
    public SetInventoryTaskInstance(IInventory inventory, Item[] itemsToPutDown, Item[] itemsToPickUp)
    {
        _inventory = inventory;
        _itemsToPutDown = itemsToPutDown;
        _itemsToPickUp = itemsToPickUp;
    }

    public override void Start()
    {
        Action();
        _done = true;
        Complete();
    }

    public override void Update() { }

    public override void Stop() { }

    public override bool IsCompleted => _done;
    
    private void Action()
    {
        foreach (Item item in _itemsToPutDown)
        {
            _inventory.AddItem(item);
        }
        
        foreach (Item item in _itemsToPickUp)
        {
            _inventory.RemoveItem(item);
        }
    }
}
