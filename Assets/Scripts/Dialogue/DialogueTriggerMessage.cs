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

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Controler>()) return;
        
        isPlayerOn = true;
        base.OnTriggerEnter(other);

        lastMessage = Instantiate(messagePrefab, messagePoint);
        lastMessage.TryGetComponent(out DialogueMessage message);
        message.CurText = table.GetTable().GetEntry(messageTextKey).Value;
    }

    public override void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.GetComponent<Controler>()) return;

        isPlayerOn = false;
        base.OnTriggerExit(other);
        Destroy(lastMessage);
    }

    public override void Action()
    {
        if (!doubleMessage | !isPlayerOn) return;
        lastMessage.TryGetComponent(out DialogueMessage message);
        message.Clear();
        message.CurText = table.GetTable().GetEntry(doubleMessageTextKey).Value;
    }
}
