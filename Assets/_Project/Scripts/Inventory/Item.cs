using UnityEngine;
using UnityEngine.Localization;

public enum ItemType
{
    Item = 0,
    Material = 1
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public ItemType type = ItemType.Item;
    public LocalizedString itemname;
    public LocalizedString description;
    public Sprite sprite;
}