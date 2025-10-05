using UnityEngine;
using Zenject;

public class Pet : DialogueTriggerMessage
{
    private DestroyableObject destroyableObject;
    [SerializeField] private Item itemForUse;
    [SerializeField] private int heal;
    
    private IInventory inventory;
    [Inject]
    private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }
    
    public void Update()
    {
        messagePoint.transform.rotation = Quaternion.identity;
    }

    public override void Start()
    {
        base.Start();
        destroyableObject = target.gameObject.GetComponent<DestroyableObject>();
    }
    
    public override void Action()
    {
        if (doubleMessage && !inventory.Items.Contains(itemForUse)) base.Action();
        if (!isPlayerOn || !inventory.Items.Contains(itemForUse)) return;
        inventory.Items.Remove(itemForUse);
        inventory.OnItemsChanged?.Invoke();
        destroyableObject.HP += heal;
    }
}
