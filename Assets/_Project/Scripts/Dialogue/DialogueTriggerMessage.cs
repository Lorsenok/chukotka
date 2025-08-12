using System;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueTriggerMessage : PickableObject
{
    [SerializeField] protected Transform messagePoint;
    [SerializeField] protected GameObject messagePrefab;
    [SerializeField] protected LocalizedStringTable table;
    [SerializeField] protected string messageTextKey;

    [SerializeField] protected bool doubleMessage;
    [SerializeField] protected string doubleMessageTextKey;

    protected GameObject lastMessage;

    protected bool isPlayerOn = false;

    public override void OnEnter(GameObject obj)
    {
        if (target != obj) return;
        
        isPlayerOn = true;
        base.OnEnter(obj);

        lastMessage = Instantiate(messagePrefab, messagePoint);
        if (table.GetTable().GetEntry(messageTextKey) == null)
        {
            Debug.LogError("Table key doesnt exist! Key: " + messageTextKey);
            return;
        }
        lastMessage.GetComponentInChildren<DialogueMessage>().CurText = table.GetTable().GetEntry(messageTextKey).Value;
    }

    public override void OnLeave(GameObject obj)
    {
        if (target != obj) return;

        isPlayerOn = false;
        base.OnLeave(obj);
        Destroy(lastMessage);
    }

    public override void Action()
    {
        if (!doubleMessage | !isPlayerOn) return;
        DialogueMessage message = lastMessage.GetComponentInChildren<DialogueMessage>();
        message.Clear();
        message.CurText = table.GetTable().GetEntry(doubleMessageTextKey).Value;
    }
}