using System.Collections.Generic;
using UnityEngine;

public class NpcRegistry : MonoBehaviour
{
    private readonly Dictionary<string, NpcDialogController> _map = new();

    private void Awake()
    {
        foreach (var npc in FindObjectsOfType<NpcDialogController>())
            _map[npc.Id] = npc;
    }

    public NpcDialogController Get(string id) =>
        _map.TryGetValue(id, out var npc) ? npc : null;
}