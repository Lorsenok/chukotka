using UnityEngine;
using Zenject;

public class SkillsUnlocker : MonoBehaviour
{
    [SerializeField] private Ability[] abilities;
    [SerializeField] private int[] abilitiesProgression;
    
    IAbilityContainer abilContainer;
    [Inject]
    void StartInit(IAbilityContainer cont)
    {
        abilContainer = cont;
    }

    void Start()
    {
        CheckUnlocks();
        Trigger.OnProgressionChanged += CheckUnlocks;
    }

    void OnDestroy()
    {
        Trigger.OnProgressionChanged -= CheckUnlocks;
    }

    void CheckUnlocks()
    {
        int pr = 0;

        if (PlayerPrefs.HasKey("progression"))
        {
            pr = (int)GameSaver.Load("progression", typeof(int));
        }
        else
        {
            pr = Trigger.Progression;
        }

        for (int i = 0; i < abilitiesProgression.Length; i++)
        {
            AbilityObject ab = new(abilities[i]);
            if (abilitiesProgression[i] <= pr)
                abilContainer.AddAbility(ab);
        }
    }
}
