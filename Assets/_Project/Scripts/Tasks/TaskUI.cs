using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class TaskUI : MonoBehaviour
{
    private List<GameObject> curTasksObj = new List<GameObject>();

    [SerializeField] private Transform grid;
    [SerializeField] private TextMeshProUGUI textPrefab;

    [SerializeField] private Menu blank;
    [SerializeField] private Menu main;

    [SerializeField] private Timer tasksShowTime;
    [SerializeField] private Timer hideTasksTime;

    private InputSystem inputSystem;
    private ITaskContainer container;
    [Inject] private void Init(IInputControler inputControler, ITaskContainer taskContainer)
    {
        inputSystem = inputControler.GetInputSystem();
        container = taskContainer;
    }

    private void OnOpen(InputAction.CallbackContext context)
    {
        blank.Open = !blank.Open;
        main.Open = !main.Open;
    }

    private void OnEnable()
    {
        inputSystem.UI.OpenTasks.performed += OnOpen;
        tasksShowTime.OnTimerEnd += OnShowTasksEnd;
        hideTasksTime.OnTimerEnd += OnHideTasksEnd;
        container.OnTasksChanged += OnTasksUpdate;
    }

    private void OnDisable()
    {
        inputSystem.UI.OpenTasks.performed -= OnOpen;
        tasksShowTime.OnTimerEnd -= OnShowTasksEnd;
        hideTasksTime.OnTimerEnd -= OnHideTasksEnd;
        container.OnTasksChanged -= OnTasksUpdate;
    }

    private bool showTasks = false;
    private void OnShowTasksEnd()
    {
        if (showTasks)
        {
            blank.Open = true;
            main.Open = false;
        }
        showTasks = false;
    }

    private bool hideTasks = false;
    private void OnHideTasksEnd()
    {
        if (hideTasks)
        {
            TasksUpdate();
            showTasks = true;
            tasksShowTime.StartTimer();
        }
        hideTasks = false;
    }

    private void TasksUpdate()
    {
        foreach (GameObject obj in curTasksObj)
        {
            Destroy(obj);
        }
        curTasksObj.Clear();

        foreach (Task task in container.Tasks)
        {
            TextMeshProUGUI text = Instantiate(textPrefab, grid);
            text.text = task.description.GetLocalizedString();
            curTasksObj.Add(text.gameObject);
        }
    }

    private void OnTasksUpdate()
    {
        hideTasks = true;
        hideTasksTime.StartTimer();
    }

    private void Update()
    {
        if (hideTasks)
        {
            blank.Open = true;
            main.Open = false;
        }
        else if (showTasks)
        {
            main.Open = true;
            blank.Open = false;
        }
    }
}
