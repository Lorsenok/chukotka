using System;
using UnityEngine;
using Zenject;

public class ReviveEffect : MonoBehaviour //It's not even an effect, should be fixed later
{
    [SerializeField] private InventoryCellChecker cellChecker;
    [SerializeField] private DestroyableObject destroyableObject;
    [SerializeField] private int set = 10;
    [SerializeField] private GameObject[] spawnOnUse;
    [SerializeField] private float cameraShakePower;
    
    private IInventory inventory;
    [Inject]
    private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }
    
    private void Update()
    {
        if (!cellChecker.HasItem || destroyableObject.HP > 0) return;
        GameSaver.StopAllSaves = false;
        inventory.Items.Remove(cellChecker.item);
        inventory.OnItemsChanged?.Invoke();
        destroyableObject.HP = set;
        destroyableObject.enabled = true;
        foreach (var obj in spawnOnUse)
        {
            Instantiate(obj, destroyableObject.transform.position, obj.transform.rotation);
        }
        CameraMovement.Shake(cameraShakePower);
    }
}
