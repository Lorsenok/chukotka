using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UI;
using Zenject;

public class MouseNotification : MonoBehaviour
{
    private class NotifItem
    {
        public LocalizedString itemname;
        public LocalizedString description;
    }
    
    public static Action<Item> OnItemNotifEnter { get; set; }
    public static Action<Item> OnItemNotifExit { get; set; }

    private List<NotifItem> notifyItems = new List<NotifItem>();

    [Header("Main")]
    [SerializeField] private RectTransform point;
    [SerializeField] private DialogueMessage nameMessage;
    [SerializeField] private DialogueMessage descriptionMessage;
    
    [Header("Panel")]
    [SerializeField] private Image panel;
    [SerializeField] private Color panelColorAppear;
    [SerializeField] private Color panelColorDisappear;
    [SerializeField] private float panelColorChangeSpeed;
    
    private InputSystem inputSystem;
    [Inject] private void Init(IInputControler inputControler)
    {
        inputSystem = inputControler.GetInputSystem();
    }

    private void OnEnable()
    {
        OnItemNotifEnter += OnItemEnter;
        OnItemNotifExit += OnItemExit;
    }

    private void OnDisable()
    {
        OnItemNotifEnter += OnItemExit;
        OnItemNotifExit -= OnItemExit;
    }

    private void OnItemEnter(Item item)
    {
        NotifItem notifItem = new NotifItem();
        notifItem.itemname = item.itemname;
        notifItem.description = item.description;
        notifyItems.Add(notifItem);
    } 

    private void OnItemExit(Item item)
    {
        foreach (NotifItem i in notifyItems.ToList())
        {
            if (i.itemname == item.itemname && i.description == item.description)
            {
                notifyItems.Remove(i);
            }
        }
    }

    private RectTransform parentCanvas;

    private void Update()
    {
        if (parentCanvas == null) parentCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        Vector2 diff = new Vector2(Screen.width, Screen.height) / parentCanvas.sizeDelta;
        point.anchoredPosition = Input.mousePosition / diff;

        bool isOnItem = !(notifyItems.Count == 0 || inputSystem.UI.Click.IsPressed());
        
        panel.color = Color.Lerp(panel.color, isOnItem ? panelColorAppear : panelColorDisappear, panelColorChangeSpeed * Time.deltaTime);
        
        if (!isOnItem)
        {
            nameMessage.Clear();
            descriptionMessage.Clear();
            return;
        }

        nameMessage.CurText = notifyItems[0].itemname.GetLocalizedString();
        descriptionMessage.CurText = notifyItems[0].description.GetLocalizedString();
    }
}
