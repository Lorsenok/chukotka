using UnityEngine;

public class AfterTask : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private GameObject[] spawn;
    [SerializeField] private GameObject[] enable;
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
            Instantiate(item, item.transform.position, item.transform.rotation);
        }
        foreach (var item in enable)
        {
            item.SetActive(true);
        }
        if (destroyAfterSpawn) Destroy(gameObject);
    }
}
