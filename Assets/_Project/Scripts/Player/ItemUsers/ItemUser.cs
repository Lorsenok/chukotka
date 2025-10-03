using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class ItemUser : MonoBehaviour
{
    [SerializeField] protected CellUser[] cellUsers;
    [SerializeField] protected Item item;
    [SerializeField] protected bool removeItemAfterUsing = true;
    [SerializeField] protected Timer delayTimer;
    protected bool hasTimerEnded = true;
    
    protected IInventory inventory;
    [Inject]
    protected virtual void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }
    
    public void Use(Item usedItem)
    {
        if (!hasTimerEnded || item != usedItem) return;

        if (removeItemAfterUsing)
        {
            inventory.Items.Remove(item);
            inventory.OnItemsChanged?.Invoke();
        }
        
        hasTimerEnded = false;
        delayTimer.StartTimer();
        
        Action();
    }

    protected virtual void Action()
    {
        
    }

    private void OnDelayEnd()
    {
        hasTimerEnded = true;
    }

    public void OnEnable()
    {
        delayTimer.OnTimerEnd += OnDelayEnd;
        foreach (CellUser cellUser in cellUsers)
        {
            cellUser.OnItemUsed += Use;
        }
    }

    public void OnDisable()
    {
        delayTimer.OnTimerEnd -= OnDelayEnd;
        foreach (CellUser cellUser in cellUsers)
        {
            cellUser.OnItemUsed -= Use;
        }
    }
}
