using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/SetInventoryTask")]
public class SetInventoryTaskConfig : TaskConfig
{
    [Tooltip("Что положить в инвентарь")] [SerializeField]
    private Item[] _itemsToPutDown;

    [Tooltip("Что достать из инвентаря")] [SerializeField]
    private Item[] _itemsToPickUp;
    
    public Item[] ItemsToPutDown => _itemsToPutDown;
    public Item[] ItemsToPickUp => _itemsToPickUp;
}
