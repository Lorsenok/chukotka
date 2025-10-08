using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueMessage : MonoBehaviour
{
    public string CurText { get; set; } = string.Empty;
    public bool EndedText { get; set; } = false;
    public bool Skip { get; set; } = false;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Timer timer;

    private int curSym = 0;

    private void OnTimerEnd()
    {
        if (curSym >= CurText.Length) return;
        
        text.text += CurText[curSym];

        if (CurText[curSym] == '<')
        {
            while (CurText[curSym] != '>')
            {
                curSym++;
                text.text += CurText[curSym];
            }
        }

        curSym++;
    }

    public void Clear()
    {
        curSym = 0;
        Skip = false;
        CurText = string.Empty;

        text.text = string.Empty;

        EndedText = false;
    }

    private void OnEnable()
    {
        timer.OnTimerEnd += OnTimerEnd;
    }

    private void OnDisable()
    {
        timer.OnTimerEnd -= OnTimerEnd;
    }

    private void Update()
    {
        if (Skip) OnTimerEnd();
    }
}
