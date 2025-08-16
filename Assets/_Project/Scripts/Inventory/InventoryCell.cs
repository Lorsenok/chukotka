using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static InventoryCell TargetedCell { get; private set; }

    public RectTransform Canvas { get; set; }
    public InventoryItemUI ItemObj { get; set; }
    public RectTransform ItemObjTransform { get; set; }

    public void Update()
    {
        if (ItemObj != null && ItemObjTransform == null)
        {
            ItemObjTransform = ItemObj.GetComponent<RectTransform>();
        }

        if (ItemObj == null)
        {
            ItemObjTransform = null;
        }
        else
        {
            ItemObj.InitialPosition =
                transform.localPosition - new Vector3(Screen.width / 2f - ItemObjTransform.sizeDelta.x, -Screen.height / 2f + ItemObjTransform.sizeDelta.y, 0f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TargetedCell = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TargetedCell = null;
    }
}