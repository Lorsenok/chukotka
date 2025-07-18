using System;
using UnityEngine;

public interface IDialogueSetter
{
    void SetTree(DialogueTree tree, Trigger trigger);
}
public class DialogueService : IDialogueSetter
{
    public static Action<DialogueTree, Trigger> OnTreeSet { get; set; }
    public static Action OnDialogueEnd { get; set; }

    public void SetTree(DialogueTree tree, Trigger trigger)
    {
        OnTreeSet?.Invoke(tree, trigger);
    }
}
