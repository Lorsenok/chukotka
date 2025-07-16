using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameLayersControler : MonoBehaviour
{
    public static int CurrentLayer { get; set; } = 0;

    [SerializeField] private int setLayerOnStart = 0;
    [SerializeField] private GameLayer[] layers;

    [SerializeField] private float speed;

    [SerializeField] private float zdistance;
    [SerializeField] private float ydistance;

    private List<List<Collider2D>> colliders = new List<List<Collider2D>>();

    private InputSystem inputSystem;
    private IGameState state;
    [Inject] private void Init(IInputControler inputControler, IGameState gameState)
    {
        inputSystem = inputControler.GetInputSystem();
        state = gameState;
    }

    private void Start()
    {
        CurrentLayer = setLayerOnStart;

        foreach (GameLayer layer in layers)
        {
            List<Collider2D> colls = new List<Collider2D> ();
            foreach (Collider2D coll in layer.GetComponentsInChildren<Collider2D>())
            {
                if (!coll.gameObject.GetComponent<GameLayerColliderLock>()) colls.Add(coll);
            }
            colliders.Add(colls);
        }
    }

    private void OnSwitchUp(InputAction.CallbackContext context)
    {
        if (GameLayerSwitchBlock.Block || state.GetCurrectState() != GameState.Game) return;
        if (CurrentLayer < layers.Length - 1) CurrentLayer++;
    }

    private void OnSwitchDown(InputAction.CallbackContext context)
    {
        if (GameLayerSwitchBlock.Block || state.GetCurrectState() != GameState.Game) return;
        if (CurrentLayer > 0) CurrentLayer--;
    }

    private void OnEnable()
    {
        inputSystem.Player.SwitchLayerUp.performed += OnSwitchUp;
        inputSystem.Player.SwitchLayerDown.performed += OnSwitchDown;
    }
    private void OnDisable()
    {
        inputSystem.Player.SwitchLayerUp.performed -= OnSwitchUp;
        inputSystem.Player.SwitchLayerDown.performed -= OnSwitchDown;
    }

    private void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].transform.position = Vector3.Lerp(layers[i].transform.position, 
                transform.position + new Vector3(0, ydistance * (i - CurrentLayer), zdistance * (i - CurrentLayer)), 
                speed * Time.deltaTime);

            for (int j = 0; j < colliders[i].Count; j++)
            {
                colliders[i][j].enabled = i == CurrentLayer;
            }
        }
    }
}
