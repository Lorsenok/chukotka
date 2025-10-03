using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static InventoryCell TargetedCell { get; private set; }

    public RectTransform Canvas { get; set; }
    public InventoryItemUI ItemObj { get; set; }
    public RectTransform ItemObjTransform { get; set; }

    [SerializeField] private Vector3 offset;
    public List<ItemType> allowedItemTypes;
    public bool lockObjectToCanvas = true;

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
            ItemObj.InitialPosition = transform.localPosition + offset + (lockObjectToCanvas ? Canvas.anchoredPosition : Vector3.zero);
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