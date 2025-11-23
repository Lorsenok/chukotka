using UnityEngine;
using Zenject;

public class SkillsUnlocker : MonoBehaviour
{
    public int needProgForDoubleJump = 1;
    public int needProgForDash = 2;
    public int needProgForCombo = 3;

    IAbilityContainer abilContainer;

    bool doubleJumpIsOpen = false;
    bool dashIsOpen = false;
    bool comboIsOpen = false;

    [Inject]
    void StartInit(IAbilityContainer cont)
    {
        abilContainer = cont;
    }

    void Start()
    {
        if (abilContainer != null)
        {
            abilContainer.Load();
        }

        if (abilContainer != null)
        {
            doubleJumpIsOpen = abilContainer.HasAbility(x => x.Jumps > 0);
            dashIsOpen = abilContainer.HasAbility(x => x.DashPower > 0);
            comboIsOpen = abilContainer.HasAbility(x => x.Combo > 0);
        }

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

        if (pr >= needProgForDoubleJump)
        {
            if (!doubleJumpIsOpen)
            {
                if (abilContainer != null)
                {
                    AbilityObject ab = new AbilityObject();
                    ab.Jumps = 1;
                    abilContainer.AddAbility(ab);
                }

                doubleJumpIsOpen = true;
            }
        }

        if (pr >= needProgForDash)
        {
            if (!dashIsOpen)
            {
                if (abilContainer != null)
                {
                    AbilityObject ab2 = new AbilityObject();
                    ab2.DashPower = 1;
                    abilContainer.AddAbility(ab2);
                }

                dashIsOpen = true;
            }
        }

        if (pr >= needProgForCombo)
        {
            if (!comboIsOpen)
            {
                if (abilContainer != null)
                {
                    AbilityObject ab3 = new AbilityObject();
                    ab3.Combo = 1;
                    abilContainer.AddAbility(ab3);
                }

                comboIsOpen = true;
            }
        }
    }
}
