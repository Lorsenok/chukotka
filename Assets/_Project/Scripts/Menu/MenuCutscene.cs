using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using Zenject;

public class MenuCutscene : MonoBehaviour
{
    [SerializeField] private Image[] imageSequence;
    [SerializeField] private DialogueMessage message;
    [SerializeField] private LocalizedStringTable table;
    [SerializeField] private string[] keys;
    [SerializeField] private float changeSpeed;
    [SerializeField] private string nextScene = "Menu";
    private int curImg = 0;

    private bool hasClicked = false;
    private void Scroll(InputAction.CallbackContext context)
    {
        hasClicked = !hasClicked;
        if (hasClicked) return;
        
        curImg++;
        if (curImg >= imageSequence.Length)
        {
            enabled = false;
            sceneChanger.ChangeScene(nextScene, SceneStartType.center);
            return;
        }
        SetMessage(keys[curImg]);
    }

    private void SetMessage(string index)
    {
        message.Clear();
        message.CurText = table.GetTable().GetEntry(index).Value;
    }
 
    private InputSystem inputSystem;
    private ISceneChanger sceneChanger;
    [Inject]
    private void Init(IInputControler input, ISceneChanger sceneChanger)
    {
        inputSystem = input.GetInputSystem();
        this.sceneChanger = sceneChanger;
    }

    private void OnEnable()
    {
        inputSystem.UI.Click.performed += Scroll;
    }

    private void OnDisable()
    {
        inputSystem.UI.Click.performed -= Scroll;
    }

    private void Start()
    {
        SetMessage(keys[curImg]);
    }

    private void Update()
    {
        for (int i = 0; i < imageSequence.Length; i++)
        {
            imageSequence[i].color = Color.Lerp(imageSequence[i].color, 
                i == curImg ? Color.white : new Color(0f, 0f, 0f, 0f), 
                changeSpeed * Time.deltaTime);
        }
    }
}
