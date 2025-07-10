using UnityEngine;

public class SpawnAfterTask : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private GameObject[] spawn;
    [SerializeField] private bool destroyAfterSpawn = true;

    private void OnEnable()
    {
        DialogueService.OnDialogueEnd += OnDialogueEnd;
    }

    private void OnDisable()
    {
        DialogueService.OnDialogueEnd -= OnDialogueEnd;
    }

    private void OnDialogueEnd(int id)
    {
        if (id != this.id) return;
        foreach (var item in spawn)
        {
            Instantiate(item, transform.position, item.transform.rotation);
        }
        if (destroyAfterSpawn) Destroy(gameObject);
    }
}
