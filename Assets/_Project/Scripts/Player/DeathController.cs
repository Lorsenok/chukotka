using UnityEngine;
using Zenject;

public class DeathController : ControllerAddition
{
    [SerializeField] private Timer timeBeforeDeath;
    [SerializeField] private Timer timeAfterDeath;
    [SerializeField] private DestroyableObject player;
    [SerializeField] private ControllerAddition[] blockControllers;
    
    [Header("Animation")]
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string anim;
    [SerializeField] private float animTime;

    private ISceneChanger sceneChanger;
    [Inject]
    private void Init(ISceneChanger sceneChanger)
    {
        this.sceneChanger = sceneChanger;
    }
    
    private bool deathConfirmed = false;
    private void OnTimerBeforeEnd()
    {
        if (!hasPlayerDied) return;
        deathConfirmed = true;
        timeAfterDeath.StartTimer();
        timeBeforeDeath.enabled = false;
        animController.PullAnimation(anim, animTime);
        foreach (ControllerAddition c in blockControllers)
            c.enabled = false;
        SpeedMultiplier = Vector2.zero;
    }

    private void OnTimerAfterEnd()
    {
        if (!deathConfirmed) return;
        timeAfterDeath.enabled = false;
        sceneChanger.LoadGame();
    }

    public void OnEnable()
    {
        timeAfterDeath.OnTimerEnd += OnTimerAfterEnd;
        timeBeforeDeath.OnTimerEnd += OnTimerBeforeEnd;
    }

    public void OnDisable()
    {
        timeAfterDeath.OnTimerEnd -= OnTimerAfterEnd;
        timeBeforeDeath.OnTimerEnd -= OnTimerBeforeEnd;
    }

    private bool hasPlayerDied = false;
    private void Update()
    {
        if (!hasPlayerDied && player.HP <= 0)
        {
            hasPlayerDied = true;
            timeBeforeDeath.StartTimer();
        }
        if (player.HP > 0) hasPlayerDied = false;
    }
}
