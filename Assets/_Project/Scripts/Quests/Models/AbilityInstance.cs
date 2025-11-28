public class AbilityInstance : TaskInstance
{
    private IAbilityContainer _abilityContainer;
    
    private Ability[] _abilities;
    private bool _done;

    public AbilityInstance(IAbilityContainer abilityContainer, Ability[] abilities)
    {
        _abilityContainer = abilityContainer;
        _abilities = abilities; 
        _taskType = TaskType.Ability;
    }
    
    public override void Start()
    {
        AddAbilities();
        _done = true;
        Complete();
    }

    public override void Update() {}

    public override void Stop() { }

    public override bool IsCompleted => _done;
    
    private void AddAbilities()
    {
        foreach (var ability in _abilities)
        {
            AbilityObject abilityObject = new AbilityObject(ability);
            _abilityContainer.AddAbility(abilityObject);
        }
    }
}
