using System.Collections.Generic;
using UnityEngine;

public class NpcRegistry : MonoBehaviour
{
    private readonly Dictionary<int, NPCDialogueController> _npcs = new();

    public void RegisterNpc(NPCDialogueController npc)
    {
        _npcs[npc.NpcId] = npc;
    }

    public void UnregisterNpc(NPCDialogueController npc)
    {
        _npcs.Remove(npc.NpcId);
    }

    public bool TryGetNpc(int npcId, out NPCDialogueController npc)
        => _npcs.TryGetValue(npcId, out npc);
}
