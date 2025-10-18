using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class ItemUser : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected CellUser[] cellUsers;
    [SerializeField] protected Item item;
    [SerializeField] protected bool removeItemAfterUsing = true;
    [SerializeField] protected Timer delayTimer;
    protected bool hasTimerEnded = true;
    
    protected IInventory inventory;
    [Inject]
    private void Init(IInventory inventory)
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

    public virtual void Action()
    {
        
    }

    private void OnDelayEnd()
    {
        hasTimerEnded = true;
    }

    public virtual void Update()
    {
        transform.position = target.position;
    }

    public void Start()
    {
        if (target == null) target = FindObjectsByType<Controller>(FindObjectsSortMode.None)[0].transform;
    }

    public void OnEnable()
    {
        if (cellUsers.Length == 0)  cellUsers = FindObjectsByType<CellUser>(FindObjectsSortMode.None);
        if (cellUsers.Length == 0)
        {
            Debug.LogError("No cell users to get!");
            enabled = false;
            return;
        }
        
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
