using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Zenject;

public class Dialogue : MonoBehaviour
{
    private DialogueTree curTree;
    private Trigger trigger;

    [Header("Setup")]
    [SerializeField] private GameState state = GameState.Cutscene;
    [SerializeField] private GameObject disableObject;
    [SerializeField] private DialogueMessage message;
    [SerializeField] private DialogueMessage messageName;
    [SerializeField] private Image messageImage;

    [Header("Panels Setup")]
    [SerializeField] private Image[] panels;
    [SerializeField] private float panelsSwitchSpeed;
    [SerializeField] private float panelsTransperency = 0.7f;

    [Header("Choice Setup")]
    [SerializeField] private DialogueChoice choice;
    [SerializeField] private char choiceCommand;

    [Header("Positions")]
    [SerializeField] private Transform messageImageLeftSidePos;
    [SerializeField] private Transform messageImageRightSidePos;
    [SerializeField] private Transform messageLeftSidePos;
    [SerializeField] private Transform messageRightSidePos;

    private bool isWorking = false;
    private bool isChoice = false;
    private int curMessage = 1;
    private TalkTaskInstance currentTalkTask;

    private IGameState gameState;
    private InputSystem input;
    [Inject]
    private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }

    private void OnEnable()
    {
        DialogueService.OnTreeSet += SetDialogueTree;
        input.UI.Skip.performed += SkipByButton;
    }

    private void OnDisable()
    {
        DialogueService.OnTreeSet -= SetDialogueTree;
        input.UI.Skip.performed -= SkipByButton;
    }

    private void SetDialogueTree(DialogueTree tree, Trigger trigger, TalkTaskInstance talkTaskInstance)
    {
        this.trigger = trigger;
        gameState.SetState(state);
        message.Clear();
        messageName.Clear();
        message.gameObject.SetActive(true);
        isWorking = true;
        curMessage = 0;
        curTree = tree;
        currentTalkTask = talkTaskInstance;
        Setup(0);
    }

    private bool skipBlock = false;
    private bool hasSkiped = false;
    public void Skip(bool skipByChoice = false)
    {
        if (!isWorking || isChoice & !skipByChoice) return;
        if (skipBlock)
        {
            skipBlock = false;
            return;
        }
        skipBlock = isChoice;
        isChoice = false;

        if (!hasSkiped && !skipByChoice)
        {
            message.Skip = true;
            hasSkiped = true;
            return;
        }

        hasSkiped = false;
        
        message.Clear();
        messageName.Clear();

        curMessage = skipByChoice ? choice.NextId : curTree.nextId[curMessage];
        Setup(curMessage);
    }

    private void Setup(int id)
    {
        if (curTree.keys.Length - 1 < curMessage)
        {
            DialogueService.OnDialogueEnd?.Invoke(currentTalkTask);
            if (trigger != null) trigger.Action();
            isWorking = false;
            gameState.SetState(GameState.Game);
            return;
        }

        messageImage.transform.position = curTree.side[id] ? messageImageRightSidePos.position : messageImageLeftSidePos.position;
        messageImage.sprite = curTree.icon[id];

        message.transform.position = curTree.side[id] ? messageRightSidePos.position : messageLeftSidePos.position;

        if (curTree.table.GetTable().GetEntry(curTree.keys[id]).Value[0] == choiceCommand)
        {
            choice.Setup(curTree.table.GetTable().GetEntry(curTree.keys[id]).Value);
            isChoice = true;
        }
        else
        {
            message.CurText = curTree.table.GetTable().GetEntry(curTree.keys[id]).Value;
            messageName.CurText = curTree.table.GetTable().GetEntry(curTree.namekeys[id]).Value;
        }
    }

    private void SkipByButton(InputAction.CallbackContext context)
    {
        Skip();
    }

    private void Update()
    {
        disableObject.SetActive(isWorking);

        if (curTree == null || curTree.keys.Length - 1 < curMessage) return;

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].color = Color.Lerp(panels[i].color, i == curTree.panels[curMessage] ? 
                new Color(panels[i].color.r, panels[i].color.g, panels[i].color.b, panelsTransperency) : 
                new Color(panels[i].color.r, panels[i].color.g, panels[i].color.b, 0f), Time.deltaTime * panelsSwitchSpeed);
        }
    }
}
