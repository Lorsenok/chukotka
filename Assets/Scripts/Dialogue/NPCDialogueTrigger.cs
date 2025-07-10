using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private CustomAnimator idle;
    [SerializeField] private CustomAnimator active;

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

    private void OnStart(DialogueTree tree)
    {
        idle.enabled = false;
        active.enabled = true;
    }

    private void OnEnd(int id)
    {
        idle.enabled = true;
        active.enabled = false;
    }
}
