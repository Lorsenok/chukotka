using System;
using UnityEngine;

public interface IDialogueSetter
{
    void SetTree(DialogueTree tree, Trigger trigger, TalkTaskInstance talkTaskInstance);
}
public class DialogueService : IDialogueSetter
{
    public static Action<DialogueTree, Trigger, TalkTaskInstance> OnTreeSet { get; set; }
    public static Action<TalkTaskInstance> OnDialogueEnd { get; set; }

    public void SetTree(DialogueTree tree, Trigger trigger, TalkTaskInstance talkTaskInstance)
    {
        OnTreeSet?.Invoke(tree, trigger, talkTaskInstance);
    }
}
