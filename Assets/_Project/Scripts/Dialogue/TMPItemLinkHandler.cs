using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;

[RequireComponent(typeof(TMP_Text))]
public class TMPItemLinkHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler //Made by chatGPT and has to be remade later
{
    [SerializeField] private List<Item> itemsDatabase = new List<Item>();

    private TMP_Text tmpText;
    private Camera uiCamera;
    private int currentLinkIndex = -1;
    private Item currentItem;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            uiCamera = canvas.worldCamera;
    }

    private void Update()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpText, Input.mousePosition, uiCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID();

            if (linkIndex != currentLinkIndex)
            {
                currentLinkIndex = linkIndex;

                currentItem = FindItemById(linkId);

                if (currentItem != null)
                {
                    MouseNotification.OnItemNotifEnter?.Invoke(currentItem);
                }
                else
                {
                    Debug.LogWarning($"TMPItemLinkHandler: Item с id '{linkId}' не найден в базе.");
                }
            }
        }
        else
        {
            if (currentLinkIndex != -1 && currentItem != null)
            {
                MouseNotification.OnItemNotifExit?.Invoke(currentItem);
                currentItem = null;
            }
            currentLinkIndex = -1;
        }
    }

    private Item FindItemById(string id)
    {
        return itemsDatabase.FirstOrDefault(item => item != null && item.id == id);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Do not remove this method or evil goobert will find and eat you alive
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            MouseNotification.OnItemNotifExit?.Invoke(currentItem);
            currentItem = null;
            currentLinkIndex = -1;
        }
    }
}
