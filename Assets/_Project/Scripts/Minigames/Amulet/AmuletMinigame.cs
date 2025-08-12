using UnityEngine;
using Zenject;

public class AmuletMinigame : Minigame
{
    [SerializeField] private Transform target;
    [SerializeField] private Task task;
    [SerializeField] private GameObject enableObjectAfterEnd;
    [SerializeField] private Fade fade;

    private ITaskContainer taskContainer;
    [Inject]
    private void Init(ITaskContainer container)
    {
        taskContainer = container;
    }

    public override void StartGame()
    {
        base.StartGame();

        taskContainer.Tasks.Add(task);
        taskContainer.OnTasksChanged?.Invoke();
        
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

    private void OnEnable()
    {
        fade.OnFadeEnd += EndGame;
    }

    private void OnDestroy()
    {
        fade.OnFadeEnd -= EndGame;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fade.StartFade();
            enabled = false;
        }
    }
}
