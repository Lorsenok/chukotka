using UnityEngine;
using Zenject;
public class AbilitiesControler : MonoBehaviour 
{ 
    [SerializeField] private ControllerAddition jump; 
    [SerializeField] private ControllerAddition dash; 
    [SerializeField] private ControllerAddition combo; 
    [SerializeField] private Controller controller; 
    private IAbilityContainer container; 
    [Inject] private void Init(IAbilityContainer container) 
    { 
        this.container = container; 
    }
    private void Awake() 
    { 
        if (container != null) container.OnAbilitiesChanged += Set; 
    } 
    private void Start() 
    { 
        container?.Load(); Set();  
    } 
    private void OnDestroy() 
    { 
        if (container != null) container.OnAbilitiesChanged -= Set; 
    } 
    private void Set() 
    { 
        if (jump == null || dash == null || combo == null || controller == null || container == null) return; 
        jump.Addition = 0; 
        dash.Addition = 0; 
        combo.Addition = 0; 
        controller.AdditionalAcceleration = 0; 
        controller.AdditionalSpeed = 0; 
        controller.AdditionalDeceleration = 0; 
        foreach (var ability in container.Abilities) 
        { 
            if (ability == null) continue; 
            jump.Addition += ability.Jumps; 
            dash.Addition += ability.DashPower; 
            combo.Addition += ability.Combo; 
            controller.AdditionalAcceleration += ability.Aceleration; 
            controller.AdditionalSpeed += ability.Speed; 
            controller.AdditionalDeceleration += ability.Deceleration; 
        } 
        jump.enabled = jump.Addition > 0; 
        dash.enabled = dash.Addition > 0; 
        combo.enabled = combo.Addition > 0; 
    } 
}