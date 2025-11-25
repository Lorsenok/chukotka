using UnityEngine;

public abstract class TaskConfig : ScriptableObject
{
    [TextArea] public string Description;

    // Фабрика: конфиг создаёт runtime-инстанс.
    // Это держит ScriptableObject как "чистые" данные.
    public abstract TaskInstance CreateInstance();
}