using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueMessage : MonoBehaviour
{
    public string CurText { get; set; } = string.Empty;
    public bool EndedText { get; set; } = false;
    public bool Skip { get; set; } = false;

    private List<DialogueLetter> dialogueLetters = new List<DialogueLetter>();
    private List<GameObject> dialogueLettersObjects = new List<GameObject>();

    [SerializeField] private GameObject textLetterPrefab;
    [SerializeField] private Transform textSorter;
    [SerializeField] private Timer timer;
    [SerializeField] private char commandSymbol;

    private int curSym = 0;

    private bool isOnCommand = false;
    private string lastCommand = string.Empty;

    private void OnTimerEnd()
    {
        if (curSym == CurText.Length) return;

        if (CurText[curSym] == commandSymbol && !isOnCommand)
        {
            curSym++;
            int commandSym = curSym;
            curSym++;
            OnTimerEnd();
            dialogueLetters[dialogueLetters.Count - 1].ApplyEffect(CurText[commandSym].ToString());

            isOnCommand = true;
            lastCommand = CurText[commandSym].ToString();

            return;
        }
        else if (CurText[curSym] == commandSymbol)
        {
            isOnCommand = false;
            curSym++;

            return;
        }

        textLetterPrefab.GetComponentInChildren<TextMeshProUGUI>().text = CurText[curSym].ToString();
        GameObject obj = Instantiate(textLetterPrefab, textSorter);
        dialogueLetters.Add(obj.GetComponentInChildren<DialogueLetter>());
        dialogueLettersObjects.Add(obj);
        if (isOnCommand) dialogueLetters[dialogueLetters.Count - 1].ApplyEffect(lastCommand);
        
        curSym++;
    }

    public void Clear()
    {
        curSym = 0;
        Skip = false;
        CurText = string.Empty;
        foreach (GameObject l in dialogueLettersObjects)
        {
            Destroy(l);
        }
        dialogueLetters.Clear();

        isOnCommand = false;
        lastCommand = string.Empty;
        EndedText = false;
    }

    private void OnEnable()
    {
        timer.OnTimerEnd += OnTimerEnd;
    }

    private void OnDisable()
    {
        timer.OnTimerEnd -= OnTimerEnd;

        textLetterPrefab.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
    }

    private void Update()
    {
        if (Skip) OnTimerEnd();
    }
}
