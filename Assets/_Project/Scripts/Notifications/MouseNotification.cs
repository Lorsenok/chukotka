using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MouseNotification : MonoBehaviour
{
    public static Action<Item> OnItemNotifEnter { get; set; }
    public static Action<Item> OnItemNotifExit { get; set; }

    private List<Item> notifyItems = new List<Item>();

    [SerializeField] private RectTransform point;
    [SerializeField] private DialogueMessage nameMessage;
    [SerializeField] private DialogueMessage descriptionMessage;

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
        notifyItems.Add(item);
    } 

    private void OnItemExit(Item item)
    {
        if (notifyItems.Contains(item)) notifyItems.Remove(item);
    }

    private RectTransform parentCanvas;

    private void Update()
    {
        if (parentCanvas == null) parentCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        Vector2 diff = new Vector2(Screen.width, Screen.height) / parentCanvas.sizeDelta;
        point.anchoredPosition = Input.mousePosition / diff;

        if (notifyItems.Count == 0 || inputSystem.UI.Click.IsPressed())
        {
            nameMessage.Clear();
            descriptionMessage.Clear();
            return;
        }

        nameMessage.CurText = notifyItems[0].itemname.GetLocalizedString();
        descriptionMessage.CurText = notifyItems[0].description.GetLocalizedString();
    }
}
