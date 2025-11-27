using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPCDialogueController : MonoBehaviour
{
    [SerializeField] private string _npcId;
    [SerializeField] private List<DialogWrapper> _dialogs;
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
    }

    private void OnDisable()
    {
        _registry.UnregisterNpc(this);
    }
    
    public void ActivateDialog(TalkTaskInstance instance)
    {
        DialogWrapper dialog = _dialogs.Find(dialog => dialog.DialogId == instance.DialogId);
        dialog.Activate(instance);
    }

    public void RemoveDialog(TalkTaskInstance instance)
    {
        DialogWrapper dialog = _dialogs.Find(dialog => dialog.DialogId == instance.DialogId);
        _dialogs.Remove(dialog);
    }

    private void DeactivateAllDialogs()
    {
        foreach (DialogWrapper dialog in _dialogs)
        {
            dialog.Deactivate();
        }
    }
}