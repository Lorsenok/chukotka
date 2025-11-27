using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/CollectTask")]
public class CollectTaskConfig : TaskConfig
{
    [SerializeField] private Item _item;
    [SerializeField] private int _requiredCount;
    
    public Item Item => _item;
    public int RequiredCount => _requiredCount; 
}