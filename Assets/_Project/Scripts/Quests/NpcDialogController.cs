using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPCDialogueController : MonoBehaviour
{
    [SerializeField] private string _npcId;
    [SerializeField] private List<DialogWrapper> _dialogs;
    
    [Inject]
    private QuestDialogEvents _dialogEvents;
    
    public string NpcId => _npcId;

    private NpcRegistry _registry;

    [Inject]
    public void Construct(NpcRegistry registry)
    {
        _registry = registry;
    }

    private void OnEnable()
    {
        _registry.RegisterNpc(this);
        DeactivateAllDialogs();
        _dialogEvents.OnDialogCompleted += RemoveDialog;
    }

    private void OnDisable()
    {
        _registry.UnregisterNpc(this);
        _dialogEvents.OnDialogCompleted -= RemoveDialog;
    }
    
    public void ActivateDialog(TalkTaskInstance instance)
    {
        DialogWrapper dialog = _dialogs.Find(dialog => dialog.DialogId == instance.DialogId);
        dialog.Activate(instance);
    }

    public void RemoveDialog(TalkTaskInstance instance)
    {
        DialogWrapper dialog = _dialogs.Find(dialog => dialog.DialogId == instance.DialogId);
        dialog.gameObject.SetActive(false);
    }

    private void DeactivateAllDialogs()
    {
        foreach (DialogWrapper dialog in _dialogs)
        {
            dialog.Deactivate();
        }
    }
}