using UnityEngine;

[CreateAssetMenu(fileName = "Trade Item", menuName = "Scriptable Objects/Trade Item")]
public class TradeItem : ScriptableObject
{
    public Item item;
    public Item itemForTrade;
}
