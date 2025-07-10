using UnityEngine;
using Zenject;

public class TaskGiver : MonoBehaviour
{
    [SerializeField] private Task task;

    private ITaskContainer container;
    [Inject] private void Init(ITaskContainer taskContainer)
    {
        container = taskContainer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Controler>()) return;

        container.Tasks.Add(task);
        container.OnTasksChanged?.Invoke();
        Destroy(gameObject);
    }
}
