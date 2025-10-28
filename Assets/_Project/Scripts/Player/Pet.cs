using UnityEngine;
using Zenject;

public class Pet : DialogueTriggerMessage
{
    [SerializeField] private Item itemForUse;
    [SerializeField] private GameObject[] spawnOnUseItem;
    [SerializeField] private int heal;
    [SerializeField] private TargetFollower targetFollower;

    private DestroyableObject destroyableObject;
    private IInventory inventory;

    [Inject]
    private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }

    public override void Start()
    {
        base.Start();

        destroyableObject = target.GetComponent<DestroyableObject>();
        var player = Object.FindFirstObjectByType<Controller>();
        if (player != null)
        {
            targetFollower.SetTarget(player.transform);
        }
    }

    public void Update()
    {
        if (messagePoint != null)
            messagePoint.transform.rotation = Quaternion.identity;
    }

    public override void Action()
    {
        if (!isPlayerOn) return;

        if (!inventory.Items.Contains(itemForUse))
        {
            if (doubleMessage && lastMessage != null)
            {
                var message = lastMessage.GetComponentInChildren<DialogueMessage>();
                if (message != null)
                {
                    message.Clear();
                    message.CurText = table.GetTable().GetEntry(doubleMessageTextKey)?.Value ?? "Message missing";
                }
            }
            return;
        }

        if (destroyableObject == null || destroyableObject.HP <= 0) return;

        inventory.Items.Remove(itemForUse);
        inventory.OnItemsChanged?.Invoke();

        destroyableObject.HP += heal;

        foreach (var spawn in spawnOnUseItem)
        {
            if (spawn != null)
                Instantiate(spawn, transform.position, spawn.transform.rotation);
        }
    }
}