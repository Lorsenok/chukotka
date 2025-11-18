using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class DeathController : ControllerAddition
{
    [SerializeField] private Timer timeBeforeDeath;
    [SerializeField] private Timer timeAfterDeath;
    [SerializeField] private DestroyableObject player;
    [SerializeField] private ControllerAddition[] blockControllers;

    [Header("Анимация")]
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string anim;
    [SerializeField] private float animTime;

    [Header("Полное здоровье")]
    [SerializeField] public int fullHP = 100; 

    private bool deathConfirmed = false;
    private bool hasPlayerDied = false;
    private bool isRespawning = false;

    private void OnTimerBeforeEnd()
    {
        if (!hasPlayerDied || isRespawning) return;

        deathConfirmed = true;
        timeAfterDeath.StartTimer();
        timeBeforeDeath.enabled = false;

        foreach (ControllerAddition c in blockControllers)
            c.enabled = false;

        SpeedMultiplier = Vector2.zero;

        if (animController != null)
            animController.PullAnimation(anim, animTime);
    }

    private void OnTimerAfterEnd()
    {
        if (!deathConfirmed) return;
        timeAfterDeath.enabled = false;

        
        player.HP = fullHP;

        
        isRespawning = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        timeAfterDeath.OnTimerEnd += OnTimerAfterEnd;
        timeBeforeDeath.OnTimerEnd += OnTimerBeforeEnd;
    }

    private void OnDisable()
    {
        timeAfterDeath.OnTimerEnd -= OnTimerAfterEnd;
        timeBeforeDeath.OnTimerEnd -= OnTimerBeforeEnd;
    }

    private void Update()
    {
        if (isRespawning) return;

        if (!hasPlayerDied && player.HP <= 0)
        {
            hasPlayerDied = true;
            timeBeforeDeath.StartTimer();
        }
    }
}