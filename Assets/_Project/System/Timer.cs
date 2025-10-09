using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Timer : MonoBehaviour
{
    public float SpeedMultiplier { get; set; } = 1f;

    [SerializeField] private GameState stateLock = GameState.Game;
    [SerializeField] private string tagname;
    [SerializeField] protected bool repeatable;

    public Action OnTimerEnd { get; set; }

    public virtual void StartTimer()
    {
        curTime = timeSet;
    }
    
    private IGameState gameState;
    [Inject] private void Init(IGameState gameState)
    {
        this.gameState = gameState;
    }

    [SerializeField] protected string savekey;

    [SerializeField] protected float timeSet;
    [SerializeField] protected float curTime;

    public void Start()
    {
        if (PlayerPrefs.HasKey(savekey)) curTime = (float)GameSaver.Load(savekey, typeof(float));
    }

    public virtual void Update()
    {
        if (gameState == null)
        {
            Debug.LogError(name + " cant get access to gameState");
            return;
        }
        if (stateLock != GameState.Any && gameState.GetCurrentState() != stateLock) return;
        if (savekey != "" && savekey != string.Empty) GameSaver.Save(savekey, curTime);
        if (curTime > 0f) curTime -= Time.deltaTime * SpeedMultiplier;
        else
        {
            OnTimerEnd?.Invoke();
            if (repeatable) StartTimer();
        }
    }
}
