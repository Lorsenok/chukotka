using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameLayersController : MonoBehaviour
{
    [SerializeField] private GameLayer[] layers;
    [SerializeField] private int current = 0;

    private InputSystem inputSystem;
    private IGameLayerChanger layerChanger;
    [Inject] private void Init(IInputControler controler, IGameLayerChanger gameLayerChanger)
    {
        current = Mathf.Clamp(current, 0, layers.Length - 1);
        inputSystem = controler.GetInputSystem();
        layerChanger = gameLayerChanger;
    }

    private void OnGameLayerSwitch(int id)
    {
        current = id;
    }

    private void OnSwitchUp(InputAction.CallbackContext context)
    {
        if (current < layers.Length - 1) layerChanger.ChangeLayer(current + 1);
    }

    private void OnSwitchDown(InputAction.CallbackContext context)
    {
        if (current > 0) layerChanger.ChangeLayer(current - 1);
    }

    private void OnEnable()
    {
        GameLayerSwitcher.OnGameLayerSwitch += OnGameLayerSwitch;
        inputSystem.Player.SwitchLayerDown.performed += OnSwitchDown;
        inputSystem.Player.SwitchLayerUp.performed += OnSwitchUp;
    }

    private void OnDisable()
    {
        GameLayerSwitcher.OnGameLayerSwitch -= OnGameLayerSwitch;
        inputSystem.Player.SwitchLayerDown.performed -= OnSwitchDown;
        inputSystem.Player.SwitchLayerUp.performed -= OnSwitchUp;
    }

    private void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].isWorking = i == current;
            layers[i].EnableBackground = i >= current;
        }
    }
}
