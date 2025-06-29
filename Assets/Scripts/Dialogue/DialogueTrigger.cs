using UnityEngine;
using UnityEngine.Localization;
using Zenject;

public class DialogueTrigger : DialogueTriggerMessage
{
    [SerializeField] private DialogueTree tree; 

    private IDialogueSetter dialogueSetter;
    [Inject] private void Init(IDialogueSetter dialogueSetter)
    {
        this.dialogueSetter = dialogueSetter;
    }

    public override void Action()
    {
        base.Action();
        if (gameState.GetCurrectState() == GameState.Game && isPlayerOn)
        {
            dialogueSetter.SetTree(tree);
        }
    }
}