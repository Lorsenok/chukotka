using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : GameButton
{

    private Button button;


    private void Awake()
    {
        
        button = GetComponent<Button>() ?? GetComponentInParent<Button>();

    }

    private new void Start()
    {
        bool gameStarted = PlayerPrefs.GetInt("GameOn", 0) == 1;

        if (button != null)
            button.interactable = gameStarted;
        else
            Debug.LogWarning("Кнопка не найдена на объекте " + gameObject.name);
    }

    public override void Action()
    {
        Debug.Log("Continue button pressed");
        if (!isMouseOn) return;
        sceneChanger.LoadGame();
        base.Action();
    }
}