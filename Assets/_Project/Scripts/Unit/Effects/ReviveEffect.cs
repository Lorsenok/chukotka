using UnityEngine;
using Zenject;

public class ReviveEffect : MonoBehaviour //It's not even an effect, should be fixed later
{
    [SerializeField] private InventoryCellChecker cellChecker;
    [SerializeField] private DestroyableObject destroyableObject;
    [SerializeField] private int set = 10;
    [SerializeField] private GameObject[] spawnOnUse;
    [SerializeField] private VisualAction visualAction;
    
    private IInventory inventory;
    [Inject]
    private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }
    
    private void Update()
    {
        if (!cellChecker.HasItem || destroyableObject.HP > 0) 
            return;
        
        GameSaver.StopAllSaves = false;
        inventory.RemoveItem(cellChecker.item);
        destroyableObject.HP = set;
        destroyableObject.enabled = true;
        foreach (var obj in spawnOnUse)
        {
            Instantiate(obj, destroyableObject.transform.position, obj.transform.rotation);
        }
        VisualActionsHandler.Action(visualAction);
    }
}
