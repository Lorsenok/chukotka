using System;

public class QuestDialogEvents
{
    // Используем простые идентификаторы (string) для гибкости.
    //TODO В будущем можно перейти на struct/enum/ID-тип.
    public event Action<TalkTaskInstance> OnDialogCompleted;
    public event Action<TalkTaskInstance> OnDialogActivated;
    public void ActivateDialog(TalkTaskInstance instance) => OnDialogActivated?.Invoke(instance);
    public void CompleteDialog(TalkTaskInstance instance) => OnDialogCompleted?.Invoke(instance);
    
    //TODO Вынести в отдельный класс
    public event Action<string> EnemyKilled;
    public event Action<string> ItemCollected;
    public event Action<string> LocationEntered;

    public void RaiseEnemyKilled(string enemyId) => EnemyKilled?.Invoke(enemyId);
    public void RaiseItemCollected(string itemId) => ItemCollected?.Invoke(itemId);
    public void RaiseLocationEntered(string locationId) => LocationEntered?.Invoke(locationId);
}