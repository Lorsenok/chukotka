using System.Collections.Generic;
using UnityEngine;

public class Trade : MonoBehaviour
{
    [SerializeField] private MenuManager menu;
    [SerializeField] private string menuName = "main";
    [SerializeField] private string blankMenuName = "blank";

    [SerializeField] private TradeCellButton cellPrefab;
    [SerializeField] private Transform grid;
    
    private List<GameObject> items = new List<GameObject>();
    
    public void Open(TradeItem[] tradeItems)
    {
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
        items = new List<GameObject>();
        foreach (TradeItem item in tradeItems)
        {
            TradeCellButton cell = Instantiate(cellPrefab, grid);
            cell.Item = item.item;
            cell.ItemForTrade = item.itemForTrade;
            
            items.Add(cell.gameObject);
            
        }
        menu.MenuOpen(menuName);
    }

    public void Close()
    {
        menu.MenuOpen(blankMenuName);
    }
}
