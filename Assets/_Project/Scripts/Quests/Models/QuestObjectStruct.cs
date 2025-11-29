using System;

[Serializable]
public struct QuestObjectStruct : IEquatable<QuestObjectStruct>
{
    public SceneType SceneType;
    public QuestObjectType QuestObjectType;

    public bool Equals(QuestObjectStruct other)
    {
        return SceneType == other.SceneType &&
               QuestObjectType == other.QuestObjectType;
    }

    public override bool Equals(object obj)
    {
        return obj is QuestObjectStruct other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int)SceneType * 397) ^ (int)QuestObjectType;
        }
    }
}

