using UnityEngine;
using Zenject;

public class DuckHuntMinigame : Minigame
{
    [SerializeField] private Transform target;
    [SerializeField] private Task task;
    [SerializeField] private GameObject enableObjectAfterEnd;

    private ITaskContainer taskContainer;
    [Inject] private void Init(ITaskContainer container)
    {
        taskContainer = container;
    }

    public override void StartGame()
    {
        base.StartGame();
        transform.position = target.position;
        transform.position -= new Vector3(0f, 0f, transform.position.z);
    }

    public override void EndGame()
    {
        base.EndGame();
        taskContainer.Tasks.Remove(task);
        taskContainer.OnTasksChanged?.Invoke();
        enableObjectAfterEnd.SetActive(true);
    }
}
