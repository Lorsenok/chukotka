using UnityEngine;

public class CollectTaskInstance : TaskInstance
{
    private readonly IInventory _inventory;

    private Item _item;
    private int _requiredCount;
    private int _currentCount;
    private bool _done;

    public CollectTaskInstance(IInventory inventory, Item item, int requiredCount)
    {
        _inventory = inventory;
        _item = item;
        _requiredCount = requiredCount;
    }

    public override void Start()
    {
        _done = false;
        Update();
        
        _inventory.OnItemsChanged += Update;
        Debug.Log("Начат сбор ресурса" + _item.description + " в количестве " + _requiredCount);
    }

    public override void Update()
    {
        int counter = 0;
        
        
        foreach (Item item in _inventory.Items)
        {
            if(item == null)
                continue;
            
            if (item.name  == _item.name) 
                counter++;
        }

        if (counter < _requiredCount)
        {
            Debug.Log("Собран нужный ресурс" + _item.description + " в количестве " + _currentCount);
            return;
        }

        _done = true;
        Complete();
    }

    public override void Stop()
    {
        _inventory.OnItemsChanged -= Update;
        Debug.Log("Закончен сбор ресурса" + _item.description + " в количестве " + _requiredCount);
    }

    public override bool IsCompleted => _done;
}