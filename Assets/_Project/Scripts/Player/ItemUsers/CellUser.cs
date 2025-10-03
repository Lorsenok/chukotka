using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CellUser : MonoBehaviour
{
    public  Action<Item> OnItemUsed { get; set; }
    
    [SerializeField] private InventoryCell cell;
    [SerializeField] private InputActionReference inputAction;
    
    private InputSystem inputProvider;
    [Inject]
    private void Init(IInputControler input)
    {
        inputProvider = input.GetInputSystem();
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        if (cell.ItemObj != null) OnItemUsed?.Invoke(cell.ItemObj.Item);
    }

    private void OnEnable()
    {
        inputAction.action.performed += OnUse;
    }

    private void OnDisable()
    {
        inputAction.action.performed -= OnUse;
    }
}
