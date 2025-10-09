using System;
using System.Collections.Generic;
using System.Linq;
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
    public static bool[] ItemsFree { get; set; }

    [Header("Inventory")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform panel;
    [SerializeField] private Transform itemsSpawnCanvas;
    [SerializeField] private InventoryItemUI itemPrefab;
    [SerializeField] private InventoryCell[] gridPositions;
    [SerializeField] private GameObject countTextPrefab;
    [SerializeField] private ItemType lastItemType = ItemType.Material;

    private ItemWithCount[] gridFilled;
    private List<InventoryItemUI> itemObjs = new();

    private IInventory inventory;

    private string[] pendingSavedCellItemNames = null; 
    private const string PlayerPrefsKey = "InventoryUI_GridMapping_v1";

    [Serializable]
    private class GridSaveData
    {
        public string[] cellItemNames;
    }

    [Inject]
    private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }

    private void Start()
    {
        canvas.worldCamera = Camera.main;

        for (int i = 0; i < gridPositions.Length; i++)
        {
            gridPositions[i].Canvas = panel.GetComponent<RectTransform>();
        }

        // try to load saved mapping from PlayerPrefs (if present)
        TryLoadGridMappingFromPlayerPrefs();

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

        // autosave mapping when UI is closed / disabled
        SaveGridMappingToPlayerPrefs();

        inventory.OnItemsChanged -= GridUpdate;
    }

    public string ExportGridMappingJson()
    {
        var data = new GridSaveData();
        data.cellItemNames = new string[gridPositions.Length];
        for (int i = 0; i < gridPositions.Length; i++)
        {
            var cell = gridPositions[i];
            var name = cell.ItemObj?.Item?.name ?? string.Empty;
            data.cellItemNames[i] = name;
        }
        return JsonUtility.ToJson(data);
    }

    public bool TryImportGridMappingJson(string json)
    {
        if (string.IsNullOrEmpty(json))
            return false;

        try
        {
            var data = JsonUtility.FromJson<GridSaveData>(json);
            if (data?.cellItemNames == null || data.cellItemNames.Length != gridPositions.Length)
                return false;

            pendingSavedCellItemNames = data.cellItemNames;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to parse grid mapping JSON: {e}");
            return false;
        }
    }

    public void SaveGridMappingToPlayerPrefs()
    {
        try
        {
            string json = ExportGridMappingJson();
            PlayerPrefs.SetString(PlayerPrefsKey, json);
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to save inventory grid mapping to PlayerPrefs: {e}");
        }
    }

    private bool TryLoadGridMappingFromPlayerPrefs()
    {
        string json = PlayerPrefs.GetString(PlayerPrefsKey, string.Empty);
        if (string.IsNullOrEmpty(json))
            return false;

        return TryImportGridMappingJson(json);
    }

    private void GridUpdate()
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

        foreach (var uiObj in itemObjs.ToArray())
        {
            if (uiObj == null)
                continue;

            if (uiObj.Item != null && newItems.TryGetValue(uiObj.Item.name, out var itemWithCount))
            {
                var text = uiObj.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                    text.text = itemWithCount.Count.ToString();

                newItems.Remove(uiObj.Item.name);
            }
            else
            {
                if (uiObj.CurCell != null)
                    uiObj.CurCell.ItemObj = null;

                Destroy(uiObj.gameObject);
            }
        }

        itemObjs.RemoveAll(obj => obj == null);

        if (pendingSavedCellItemNames != null && pendingSavedCellItemNames.Length == gridPositions.Length)
        {
            var savedNames = new HashSet<string>(pendingSavedCellItemNames.Where(s => !string.IsNullOrEmpty(s)));

            var currentNames = new HashSet<string>(newItems.Keys);

            if (savedNames.SetEquals(currentNames))
            {
                bool valid = true;
                for (int i = 0; i < pendingSavedCellItemNames.Length; i++)
                {
                    var name = pendingSavedCellItemNames[i];
                    if (string.IsNullOrEmpty(name)) continue;
                    if (!newItems.TryGetValue(name, out var itemWithCount))
                    {
                        valid = false;
                        break;
                    }
                    var itemType = itemWithCount.Item.type;
                    var cell = gridPositions[i];
                    if (cell.allowedItemTypes == null || !cell.allowedItemTypes.Contains(itemType))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    for (int i = 0; i < pendingSavedCellItemNames.Length; i++)
                    {
                        var name = pendingSavedCellItemNames[i];
                        if (string.IsNullOrEmpty(name)) continue;
                        if (!newItems.TryGetValue(name, out var itemWithCount)) continue; 
                        var cell = gridPositions[i];

                        InventoryItemUI obj = Instantiate(itemPrefab, cell.transform.position, Quaternion.identity, itemsSpawnCanvas);
                        itemObjs.Add(obj);
                        obj.GetComponentInChildren<Image>().sprite = itemWithCount.Item.sprite;
                        obj.CurCell = cell;
                        obj.Item = itemWithCount.Item;

                        var countText = Instantiate(countTextPrefab, obj.transform).GetComponentInChildren<TextMeshProUGUI>();
                        countText.text = itemWithCount.Count.ToString();

                        cell.ItemObj = obj;

                        newItems.Remove(name);
                    }

                    pendingSavedCellItemNames = null;
                }
                else
                {
                    Debug.Log("Saved inventory grid mapping invalid (cell type mismatch). Skipping load.");
                    pendingSavedCellItemNames = null;
                }
            }
            else
            {
                Debug.Log("Saved inventory grid mapping doesn't match current inventory items. Skipping load.");
                pendingSavedCellItemNames = null;
            }
        }

        foreach (var kvp in newItems)
        {
            ItemWithCount itemWithCount = kvp.Value;

            InventoryCell freeCell = Array.Find(gridPositions, c =>
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

    public override void Update()
    {
        base.Update();

        ItemsFree = new bool[(int)lastItemType + 1];

        foreach (var cell in gridPositions)
        {
            if (cell.ItemObj == null && cell.allowedItemTypes != null)
            {
                foreach (var allowedType in cell.allowedItemTypes)
                {
                    ItemsFree[(int)allowedType] = true;
                }
            }
        }
    }

    public static bool HasFreeSpaceFor(ItemType type)
    {
        if (ItemsFree == null || (int)type >= ItemsFree.Length)
            return false;

        return ItemsFree[(int)type];
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
