using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private CustomAnimator idle;
    [SerializeField] private CustomAnimator active;
    [SerializeField] private Controller playerController; 

    public void OnEnable()
    {
        DialogueService.OnTreeSet += OnStart;
        DialogueService.OnDialogueEnd += OnEnd;
    }

    public void OnDisable()
    {
        DialogueService.OnTreeSet -= OnStart;
        DialogueService.OnDialogueEnd -= OnEnd;
    }

    private void OnStart(DialogueTree tree, Trigger trigger, TalkTaskInstance talkTaskInstance)
    {
        idle.enabled = false;
        active.enabled = true;

        if (playerController)
            playerController.CanMove = false;
    }

    private void OnEnd(TalkTaskInstance talkTaskInstance)
    {
        idle.enabled = true;
        active.enabled = false;

        if (playerController)
            playerController.CanMove = true;
    }
}
