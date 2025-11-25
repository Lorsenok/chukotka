using UnityEngine;

public class DialogWrapper : MonoBehaviour
{
    [SerializeField] private string _dialogId;
    [SerializeField] private DialogueTrigger _trigger;
    
    public string DialogId => _dialogId;

    public void Activate()
    {
        _trigger.gameObject.SetActive(true);
    }
    
    public void Deactivate()
    {
        _trigger.gameObject.SetActive(false);
    }
}
