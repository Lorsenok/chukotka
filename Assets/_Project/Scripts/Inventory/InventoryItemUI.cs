using UnityEngine.EventSystems;

public class InventoryItemUI : GrapableObject
{
    public InventoryCell CurCell { get; set; }
    public Item Item { get; set; }

    public void Switch(InventoryCell cell)
    {
        if (!cell.allowedItemTypes.Contains(CurCell.ItemObj.Item.type)) return;
        InventoryItemUI i = null;
        if (cell.ItemObj != null)
        {
            i = cell.ItemObj;
            i.CurCell = CurCell;
        }
        cell.ItemObj = this;
        CurCell.ItemObj = i;
        CurCell = cell;
    }

    public override void OnGrapEnd()
    {
        base.OnGrapEnd();
        if (InventoryCell.TargetedCell != null) Switch(InventoryCell.TargetedCell);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        MouseNotification.OnItemNotifEnter?.Invoke(Item);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        MouseNotification.OnItemNotifExit?.Invoke(Item);
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            MouseNotification.OnItemNotifExit?.Invoke(Item);
        }
    }
}