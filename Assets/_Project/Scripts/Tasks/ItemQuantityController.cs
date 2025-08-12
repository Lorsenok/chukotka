using UnityEngine;
using Zenject;

public class ItemQuantityController : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity;

    [SerializeField] private Trigger trigger;

    private IInventory inventory;
    private ITaskContainer container;
    [Inject] private void Init(IInventory inventory, ITaskContainer container)
    {
        this.inventory = inventory;
        this.container = container;
    }

    private void OnEnable()
    {
        inventory.OnItemsChanged += Check;
    }

    private void OnDisable()
    {
        inventory.OnItemsChanged -= Check;
    }

    private void Start()
    {
        Check();
    }

    private void Check()
    {
        int cnt = 0;
        foreach (Item item in inventory.Items)
        {
            if (item.name == this.item.name) cnt++;
        }
        if (cnt < quantity) return;

        trigger.Action();

        Destroy(gameObject);
    }
}
