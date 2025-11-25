using UnityEngine;
using Zenject;

public class NPCDialogueController : MonoBehaviour
{
    [SerializeField] private int _npcId;
    public int NpcId => _npcId;

    private NpcRegistry _registry;

    [Inject]
    public void Construct(NpcRegistry registry)
    {
        _registry = registry;
    }

    private void OnEnable()
    {
        _registry.RegisterNpc(this);
    }

    private void OnDisable()
    {
        _registry.UnregisterNpc(this);
    }
}