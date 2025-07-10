using System;
using UnityEngine;

public interface IDialogueSetter
{
    void SetTree(DialogueTree tree);
}
public class DialogueService : IDialogueSetter
{
    public static Action<DialogueTree> OnTreeSet { get; set; }
    public static Action<int> OnDialogueEnd { get; set; }

    public void SetTree(DialogueTree tree)
    {
        OnTreeSet?.Invoke(tree);
    }
}
