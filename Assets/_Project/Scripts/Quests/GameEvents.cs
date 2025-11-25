using System;

public static class GameEvents
{
    // Используем простые идентификаторы (string) для гибкости.
    //TODO В будущем можно перейти на struct/enum/ID-тип.
    public static event Action<string> EnemyKilled;
    public static event Action<string> ItemCollected;
    public static event Action<string> DialogueCompleted;
    public static event Action<string> LocationEntered;

    public static void RaiseEnemyKilled(string enemyId) => EnemyKilled?.Invoke(enemyId);
    public static void RaiseItemCollected(string itemId) => ItemCollected?.Invoke(itemId);
    public static void RaiseDialogueCompleted(string npcId) => DialogueCompleted?.Invoke(npcId);
    public static void RaiseLocationEntered(string locationId) => LocationEntered?.Invoke(locationId);
}