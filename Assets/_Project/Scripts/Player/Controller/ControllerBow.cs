using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ControllerBow : ControllerAddition
{
    public int Arrows { get; set; } = 10;
    
    [SerializeField] private ControllerAddition[] controllersBlock;
    [SerializeField] private Gun gun;
    [SerializeField] private float shootPower = 20f;
    [SerializeField] private float timeToLoadSet;
    [SerializeField] private float startTimeToLoad;
    private float curLoadTime = 0f;
    [SerializeField] private Timer perLoadTimer;
    [SerializeField] private float loadSpeedMultiplier;
    [SerializeField] private GameObject[] spawnOnShoot;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("Animations")]
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string holdAnim;
    [SerializeField] private float holdAnimTime = 0.1f;
    [SerializeField] private string shootAnim;
    [SerializeField] private float shootAnimTime;
    
    private InputSystem input;
    private IGameState gameState;
    [Inject] private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }

    private void OnEnable()
    {
        input.Player.Shoot.performed += Shoot;
        perLoadTimer.OnTimerEnd += OnLoadEnd;
    }

    private void OnDisable()
    {
        input.Player.Shoot.performed -= Shoot;
        perLoadTimer.OnTimerEnd -= OnLoadEnd;
    }

    private void Update()
    {
        SpeedMultiplier = new Vector2(isHolding ? loadSpeedMultiplier : 1.0f, 1f);

        foreach (var controller in controllersBlock)
        {
            controller.Block = isHolding;
        }
        
        if (isHolding)
        {
            spr.flipX = input.UI.MousePosition.ReadValue<Vector2>().x < Screen.width / 2;
            animController.PullAnimation(holdAnim, holdAnimTime);
            curLoadTime += Time.deltaTime;
            curLoadTime = Mathf.Clamp(curLoadTime, 0f, timeToLoadSet);
        }

        text.text = Arrows.ToString();
    }

    private bool isHoldingWhileLoad = false;
    private bool isHolding = false;
    private void Shoot(InputAction.CallbackContext context)
    {

        if (!gun || !gun.gameObject.activeInHierarchy) return;

        Controller playerController = GetComponent<Controller>();
        if (playerController != null && !playerController.CanMove) return;

        isHoldingWhileLoad = !isHoldingWhileLoad;
        if (isLoading || Arrows <= 0) return;

        isHolding = !isHolding;
        if (!isHoldingWhileLoad && isHolding)
        {
            isHoldingWhileLoad = false;
            isHolding = false;
            return;
        }
        if (!isHolding)
        {
            gun.Shoot(shootPower * (1 / timeToLoadSet * (curLoadTime + startTimeToLoad)));
            curLoadTime = 0f;
            perLoadTimer.StartTimer();
            isLoading = true;

            foreach (GameObject obj in spawnOnShoot)
            {
                Instantiate(obj, transform.position, obj.transform.rotation);
            }

            Arrows--;
            SaveArrows();
            animController.PullAnimation(shootAnim, shootAnimTime);
        }
    }


    public void SaveArrows()
    {
        GameSaver.Save("arrows", Arrows);
    }

    private bool isLoading = false;
    private void OnLoadEnd()
    {
        isLoading = false;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("arrows"))
        {
            Arrows = (int)GameSaver.Load("arrows", typeof(int));
        }
    }
}
