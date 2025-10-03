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
    [SerializeField] private Transform itemsSpawnCanvas;
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

    private void GridUpdate() //this function was made with chatgpt and has to be remade later 
    {
        Dictionary<string, ItemWithCount> newItems = new();
        foreach (Item item in inventory.Items)
        {
            if (newItems.TryGetValue(item.name, out var existing))
            {
                existing.Count++;
            }
            else
            {
                newItems[item.name] = new ItemWithCount(item);
            }
        }

        foreach (var uiObj in itemObjs)
        {
            if (uiObj.Item != null && newItems.TryGetValue(uiObj.Item.name, out var itemWithCount))
            {
                var text = uiObj.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                    text.text = itemWithCount.Count.ToString();

                newItems.Remove(uiObj.Item.name);
            }
            else
            {
                Destroy(uiObj.gameObject);
                uiObj.CurCell.ItemObj = null;
            }
        }

        itemObjs.RemoveAll(obj => obj == null);

        foreach (var kvp in newItems)
        {
            ItemWithCount itemWithCount = kvp.Value;

            InventoryCell freeCell = System.Array.Find(gridPositions, c =>
                c.ItemObj == null &&
                c.allowedItemTypes != null && 
                c.allowedItemTypes.Contains(itemWithCount.Item.type));

            if (freeCell == null)
            {
                Debug.Log("Inventory cells are full, but there are still items");
                continue;
            }

            InventoryItemUI obj = Instantiate(itemPrefab, freeCell.transform.position, Quaternion.identity, itemsSpawnCanvas);
            itemObjs.Add(obj);
            obj.GetComponentInChildren<Image>().sprite = itemWithCount.Item.sprite;
            obj.CurCell = freeCell;
            obj.Item = itemWithCount.Item;

            var countText = Instantiate(countTextPrefab, obj.transform).GetComponentInChildren<TextMeshProUGUI>();
            countText.text = itemWithCount.Count.ToString();

            freeCell.ItemObj = obj;
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
