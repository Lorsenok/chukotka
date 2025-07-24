using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

public class NoteNotification : MonoBehaviour
{
    [SerializeField] private LocalizedStringTable table;
    [SerializeField] private string key;

    [SerializeField] private DialogueMessage message;

    private IInventory inventory;
    [Inject] private void Init(IInventory inventory)
    {
        this.inventory = inventory;
    }

    private List<Item> prevItems = new List<Item>();

    private void OnEnable()
    {
        inventory.OnItemsChanged += Notif;
    }

    private void OnDisable()
    {
        inventory.OnItemsChanged -= Notif;
    }

    private void Start()
    {
        foreach (var item in inventory.Items)
        {
            prevItems.Add(item);
        } 
    }

    private void Notif()
    {
        if (inventory.Items.Count > prevItems.Count)
        {
            message.Clear();
            message.CurText = table.GetTable().GetEntry(key).Value;
        }

        prevItems.Clear();
        foreach (var item in inventory.Items)
        {
            prevItems.Add(item);
        }
    }
}
