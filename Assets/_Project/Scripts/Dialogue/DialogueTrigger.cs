using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.InputSystem;
using Zenject;

public class DialogueTrigger : DialogueTriggerMessage
{
    [SerializeField] private DialogueTree tree;
    [SerializeField] private Trigger trigger;

    private IDialogueSetter dialogueSetter;
    private InputAction dialogueAction;

    [Inject]
    private void Init(IDialogueSetter dialogueSetter, IInputControler inputControler)
    {
        this.dialogueSetter = dialogueSetter;
        dialogueAction = inputControler.GetInputSystem().Player.Dialogue;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (input != null)
            input.Player.Use.performed -= OnPickupButton;

        dialogueAction.performed += OnDialogueButton;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        dialogueAction.performed -= OnDialogueButton;
    }

    private void OnDialogueButton(InputAction.CallbackContext context)
    {
        Action();
    }

    public override void Action()
    {
        base.Action();
        if (gameState.GetCurrentState() == GameState.Game && isPlayerOn)
        {
            dialogueSetter.SetTree(tree, trigger);
        }
    }
}