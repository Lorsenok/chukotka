using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterController : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] private string command;
    [SerializeField] private string endCommand;

    protected List<int> currentCharacters = new();
    protected List<float> currentCharactersExistTime = new();

    public virtual void OnEnable()
    {
        text.OnPreRenderText += TextUpdate;
    }

    public virtual void OnDisable()
    {
        text.OnPreRenderText -= TextUpdate;
    }

    public virtual void TextUpdate(TMP_TextInfo info)
    {
        if (info.characterCount == 0)
        {
            currentCharacters.Clear();
            currentCharactersExistTime.Clear();
            return;
        }

        currentCharacters.Clear();
        currentCharactersExistTime.Clear();
        bool isOnCommand = false;
        int visibleCharacterIndex = 0; 

        for (int i = 0; i < text.text.Length; i++)
        {
            if (text.text[i] == '<')
            {
                int commandStart = i;
                while (i < text.text.Length && text.text[i] != '>')
                {
                    i++;
                }
                if (i >= text.text.Length) break;

                string curCommand = text.text.Substring(commandStart, i - commandStart + 1);

                if (curCommand == command) isOnCommand = true;
                else if (curCommand == endCommand) isOnCommand = false;

                continue;
            }

            if (isOnCommand && visibleCharacterIndex < info.characterInfo.Length)
            {
                currentCharacters.Add(visibleCharacterIndex);
                currentCharactersExistTime.Add(0f);
            }

            visibleCharacterIndex++;
        }
    }

    public virtual void Update()
    {
        for (int i = 0; i < currentCharactersExistTime.Count; i++) 
        {
            currentCharactersExistTime[i] += Time.deltaTime;
        }
    }

    public virtual void Start()
    {
        TextUpdate(text.textInfo);
    }
}