using System;
using System.Collections.Generic;
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

    private void Start()
    {
        container.Load();
    }

    private void Awake()
    {
        container.OnAbilitiesChanged += Set;
    }

    private void OnDestroy()
    {
        container.OnAbilitiesChanged -= Set;
    }

    private void Set()
    {
        jump.Addition = 0;
        combo.Addition = 0;
        dash.Addition = 0;
        
        foreach (var ability in container.Abilities)
        {
            jump.Addition += ability.Jumps;
            combo.Addition += ability.Combo;
            dash.Addition += ability.DashPower;
            
            controller.AdditionalAcceleration += ability.Aceleration;
            controller.AdditionalSpeed += ability.Speed;
            controller.AdditionalDeceleration += ability.Deceleration;
        }
    }
}
