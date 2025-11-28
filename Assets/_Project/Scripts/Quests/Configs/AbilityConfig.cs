using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/Abilities")]
public class AbilityConfig : TaskConfig
{
    [SerializeField] private Ability[] abilities;
    
    public Ability[] Abilities => abilities;
}
