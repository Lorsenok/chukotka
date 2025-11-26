using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NpcRegistry : MonoBehaviour
{
    [Inject] private readonly QuestDialogEvents _dialogEvents;

    private readonly Dictionary<string, NPCDialogueController> _npcs = new();

    private void Start()
    {
        _dialogEvents.OnDialogActivated += CheckActivateDialog;
        
        if (_dialogEvents.CurrentActiveDialog != null)
            CheckActivateDialog(_dialogEvents.CurrentActiveDialog);
    }

    private void OnDisable()
    {
        _dialogEvents.OnDialogActivated -= CheckActivateDialog;
    }

    public void RegisterNpc(NPCDialogueController npc)
    {
        _npcs[npc.NpcId] = npc;
    }

    public void UnregisterNpc(NPCDialogueController npc)
    {
        _npcs.Remove(npc.NpcId);
    }

    private void CheckActivateDialog(TalkTaskInstance instance)
    {
        if (_npcs.TryGetValue(instance.NpcId, out var npc))
            npc.ActivateDialog(instance);
    }
}