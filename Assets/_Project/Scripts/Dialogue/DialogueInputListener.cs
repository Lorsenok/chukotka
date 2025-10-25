using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueButtonListener : MonoBehaviour
{
    [SerializeField] private DialogueTriggerMessage[] npcs; 
    [SerializeField] private InputActionAsset inputActions; 

    private InputAction useDialogueAction;

    private void Awake()
    {
        useDialogueAction = inputActions.FindAction("Dialogue");
        useDialogueAction.performed += OnDialoguePressed;
        useDialogueAction.Enable();
    }

    private void OnDestroy()
    {
        useDialogueAction.performed -= OnDialoguePressed;
        useDialogueAction.Disable();
    }

    private void OnDialoguePressed(InputAction.CallbackContext context)
    {
        foreach (var npc in npcs)
        {
            if (npc.IsPlayerOn)
            {
                npc.Action(); 
                break; 
            }
        }
    }
}