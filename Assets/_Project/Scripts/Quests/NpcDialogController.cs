using UnityEngine;
using Zenject;

public class NpcDialogController : MonoBehaviour
{
    [SerializeField] private string npcId;

    private DialogueTree _currentDialog;
    private IDialogueSetter _dialogueSetter;
    private IGameState _gameState;

    public string Id => npcId;

    [Inject]
    private void Construct(IDialogueSetter dialogueSetter, IGameState gameState)
    {
        _dialogueSetter = dialogueSetter;
        _gameState = gameState;
    }
    
    public void SetDialog(DialogueTree dialog)
    {
        _currentDialog = dialog;
    }
    
    public void ActivateDialog(string dialogId)
    {
        // Локальный поиск диалога, например из массива на NPC
    }

    public void Interact()
    {
        if (_currentDialog == null)
            return;

        // запуск системы диалога
        // if (_gameState.GetCurrentState() == GameState.Game && isPlayerOn)
        // {
        //     _dialogueSetter.SetTree(_currentDialog, trigger);
        // }
        //
        // DialogUI.Instance.StartDialog(_currentDialog);
    }
}