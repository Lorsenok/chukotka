using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemWithCount
{
    public int Count { get; set; } = 1;
    public Item Item { get; set; }

    public ItemWithCount(Item item)
    {
        Item = item;
    }
}

public class InventoryUI : GameMenu
{
    [Header("Inventory")]
    [SerializeField] private Transform canvas;
    [SerializeField] private InventoryItemUI itemPrefab;
    [SerializeField] private InventoryCell[] gridPositions;
    [SerializeField] private GameObject countTextPrefab;

    private ItemWithCount[] gridFilled;
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

        GridUpdate();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        inventory.OnItemsChanged += GridUpdate;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        inventory.OnItemsChanged -= GridUpdate;
    }

    private void GridUpdate()
    {
        gridFilled = new ItemWithCount[gridPositions.Length];
        ItemWithCount[] filled = new ItemWithCount[inventory.Items.Count];

        for (int i = 0; i < inventory.Items.Count; i++)
        {
            bool contains = false;
            foreach (ItemWithCount item in filled)
            {
                if (item == null) continue;
                if (item.Item.name == inventory.Items[i].name)
                {
                    item.Count++;
                    contains = true;
                }
            }
            if (!contains) filled[i] = new ItemWithCount(inventory.Items[i]);
        }

        int y = 0;
        for (int i = 0; i < filled.Length; i++)
        {
            if (filled[i] != null)
            {
                gridFilled[y] = filled[i];
                y++;
            }
        }

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
            obj.GetComponentInChildren<Image>().sprite = gridFilled[i].Item.sprite;
            obj.CurCell = gridPositions[i];
            obj.Item = gridFilled[i].Item;
            Instantiate(countTextPrefab, obj.transform).GetComponentInChildren<TextMeshProUGUI>().text = gridFilled[i].Count.ToString();

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
