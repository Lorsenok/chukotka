using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public LocalizedString itemname;
    public LocalizedString description;
    public Sprite sprite;
}