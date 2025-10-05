using UnityEngine;

public class ArrowItem : DialogueTriggerMessage
{
    private ControllerBow bow;

    public void Update()
    {
        messagePoint.transform.rotation = Quaternion.identity;
    }

    public override void Start()
    {
        base.Start();
        bow = target.gameObject.GetComponent<ControllerBow>();
    }
    
    public override void Action()
    {
        base.Action();
        if (!isPlayerOn) return;
        bow.Arrows++;
        bow.SaveArrows();
        Destroy(gameObject);
    }
}
