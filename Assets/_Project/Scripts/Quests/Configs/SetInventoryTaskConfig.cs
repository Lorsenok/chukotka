using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/SetInventoryTask")]
public class SetInventoryTaskConfig : TaskConfig
{
    [SerializeField] private InventoryItemTrigger _inventoryItemTrigger;
    
    public InventoryItemTrigger InventoryItemTrigger => _inventoryItemTrigger;
}
