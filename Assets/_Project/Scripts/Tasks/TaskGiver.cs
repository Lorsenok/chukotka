using UnityEngine;
using Zenject;

public class TaskGiver : MonoBehaviour
{
    [SerializeField] private Task[] taskGive;
    [SerializeField] private Task[] taskTake;

    private ITaskContainer container;
    [Inject] private void Init(ITaskContainer taskContainer)
    {
        container = taskContainer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Controller>()) return;

        foreach (Task task in taskGive) container.Tasks.Add(task);
        foreach (Task task in taskTake) if (container.Tasks.Contains(task)) container.Tasks.Remove(task);
        container.OnTasksChanged?.Invoke();

        Destroy(gameObject);
    }
}
