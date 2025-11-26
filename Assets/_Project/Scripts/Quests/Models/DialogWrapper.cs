using System;
using UnityEngine;

public class DialogWrapper : MonoBehaviour
{
    [SerializeField] private string _dialogId;
    [SerializeField] private DialogueTrigger _trigger;
    
    private TalkTaskInstance _taskInstance;

    public event Action OnDialogEnded;
    
    public string DialogId => _dialogId;

    public void Activate(TalkTaskInstance taskInstance)
    {
        _taskInstance = taskInstance;
        _trigger.gameObject.SetActive(true);
        _trigger.OnDialogueEnded += OnDialogEnded;
    }
    
    public void Deactivate()
    {
        _trigger.gameObject.SetActive(false);
        _trigger.OnDialogueEnded -= OnDialogEnded;
    }
}
