using UnityEngine;
using Zenject;

public class Pet : DialogueTriggerMessage
{
    private DestroyableObject destroyableObject;
    [SerializeField] private Item itemForUse;
    [SerializeField] private GameObject[] spawnOnUseItem;
    [SerializeField] private int heal;
    [SerializeField] private TargetFollower targetFollower;
    
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
        targetFollower.SetTarget(FindObjectsByType<Controller>(FindObjectsSortMode.None)[0].transform);
    }

    public override void Action()
    {
        if (doubleMessage && !inventory.Items.Contains(itemForUse))
        {
            if (isPlayerOn)
            {
                DialogueMessage message = lastMessage.GetComponentInChildren<DialogueMessage>();
                message.Clear();
                message.CurText = table.GetTable().GetEntry(doubleMessageTextKey).Value;
            }
            return;
        }

        if (!isPlayerOn || !inventory.Items.Contains(itemForUse)) return;

        inventory.Items.Remove(itemForUse);
        inventory.OnItemsChanged?.Invoke();
        destroyableObject.HP += heal;

        foreach (var spawn in spawnOnUseItem)
        {
            Instantiate(spawn, transform.position, spawn.transform.rotation);
        }
    }
    public override void OnEnable()
    {
     
    }

    public override void OnDisable()
    {
        
    }
}
