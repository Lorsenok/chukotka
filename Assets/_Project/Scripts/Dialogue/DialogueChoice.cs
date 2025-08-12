using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueChoice : MonoBehaviour
{
    public bool IsWorking { get; set; } = false;
    public int NextId { get; set; } = 0;

    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Transform messagesSorter;
    [SerializeField] private DialogueButton choiceButtonPrefab;
    [SerializeField] private char command;

    private List<DialogueButton> buttons = new List<DialogueButton>();

    public void Setup(string text)
    {
        DialogueMessage lastMessage = null;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == command)
            {
                buttons.Add(Instantiate(choiceButtonPrefab, messagesSorter).GetComponent<DialogueButton>());
                lastMessage = buttons[buttons.Count - 1].GetComponent<DialogueMessage>();

                List<char> syms = new List<char>();
                for (int j = 0; j < int.Parse(text[i + 1].ToString()); j++)
                {
                    syms.Add(text[i + 2 + j]);
                }
                string cnt = new(syms.ToArray());
                buttons[buttons.Count - 1].Id = int.Parse(cnt);
                i += int.Parse(text[i + 1].ToString()) + 1;
            }
            else if (lastMessage != null)
            {
                lastMessage.CurText += text[i];
            }
        }
    }

    private void OnEnable()
    {
        DialogueButton.OnButtonPressed += Skip;
    }

    private void OnDisable()
    {
        DialogueButton.OnButtonPressed -= Skip;
    }

    private void Skip(int id)
    {
        NextId = id;

        dialogue.Skip(skipByChoice: true);
        foreach (DialogueButton button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
    }
}
