using UnityEngine;
using Zenject;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] private Item[] itemGive;
    [SerializeField] private Item[] itemTake;

    private IInventory container;
    [Inject]
    private void Init(IInventory inventory)
    {
        container = inventory;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Controler>()) return;

        foreach (Item item in itemGive) container.Items.Add(item);
        foreach (Item item in itemTake) if (container.Items.Contains(item)) container.Items.Remove(item);
        container.OnItemsChanged?.Invoke();

        Destroy(gameObject);
    }
}
