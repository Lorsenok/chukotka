using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryUI : GameMenu
{
    [Header("Inventory")]
    [SerializeField] private Transform canvas;
    [SerializeField] private InventoryItemUI itemPrefab;
    [SerializeField] private InventoryCell[] gridPositions;

    private Item[] gridFilled;
    private List<InventoryItemUI> itemObjs = new();

    private IInventory inventory;

    [Inject]
    private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }

    private void Start()
    {
        for (int i = 0; i < gridPositions.Length; i++)
        {
            gridPositions[i].Canvas = canvas.GetComponent<RectTransform>();
        }

        gridFilled = new Item[gridPositions.Length]; 
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if (i == gridFilled.Length) break;
            gridFilled[i] = inventory.Items[i];
        }

        GridUpdate();
    }

    private void GridUpdate()
    {
        foreach (InventoryItemUI obj in itemObjs)
        {
            Destroy(obj.gameObject);
        }

        itemObjs = new();

        for (int i = 0; i < gridPositions.Length; i++)
        {
            if (gridFilled[i] == null) continue;

            InventoryItemUI obj = Instantiate(itemPrefab, gridPositions[i].transform.position, Quaternion.identity, canvas);
            itemObjs.Add(obj);
            obj.GetComponentInChildren<Image>().sprite = gridFilled[i].sprite;
            obj.CurCell = gridPositions[i];
            obj.Item = gridFilled[i];

            gridPositions[i].ItemObj = obj;
        }
    }

    /* Item storage testing
    public override void Update()
    {
        base.Update();
        string message = "";
        foreach (InventoryCell cell in gridPositions)
        {
            if (cell.ItemObj == null)
                message += " ### ";
            else message += " +++ ";
        }
        Debug.Log(message);
    }*/
}
