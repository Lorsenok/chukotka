using System;
using UnityEngine;

public class Spirit : MonoBehaviour //govno yebanoye
{
    [Header("Aura Material")]
    [SerializeField] private Material auramat;
    [SerializeField] private string auramatValueKey;
    [SerializeField] private float auramatValueChangeSpeed;
    
    [Header("Material")]
    [SerializeField] private Material mat;
    [SerializeField] private string matValueKey;
    [SerializeField] private float matValueChangeSpeed;
    
    [Header("Player")]
    [SerializeField] private Controller player;
    [SerializeField] private Timer stopPlayerTimer;
    [SerializeField] private Collider2D stopTrigger;
    
    [Header("Shake and Flash")]
    [SerializeField] private Timer shakeDelayTimer;
    [SerializeField] private float shakePower;
    [SerializeField] private Timer flashDelayTimer;
    [SerializeField] private int flashCount;
    
    [Header("After Animation")]
    [SerializeField] private GameObject[] enableObjects;
    [SerializeField] private Timer destroyTimer;
    
    private bool isWorking = false;

    private void OnEnable()
    {
        flashDelayTimer.OnTimerEnd += OnFlash;
        shakeDelayTimer.OnTimerEnd += OnShake;
        stopPlayerTimer.OnTimerEnd += OnStopPlayerEnd;
        destroyTimer.OnTimerEnd += OnDone;
    }

    private void OnDisable()
    {
        flashDelayTimer.OnTimerEnd -= OnFlash;
        shakeDelayTimer.OnTimerEnd -= OnShake;
        stopPlayerTimer.OnTimerEnd -= OnStopPlayerEnd;
        destroyTimer.OnTimerEnd -= OnDone;
    }

    private void OnDone()
    {
        if (isWorking) Destroy(gameObject);
    }
    
    private void OnStopPlayerEnd()
    {
        if (isWorking)
        {
            player.CanMove = true;
            foreach (GameObject obj in enableObjects)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnShake()
    {
        if (!player.CanMove)
        {
            CameraMovement.Shake(shakePower);
        }
    }

    private bool isLastFlash = false;
    private void OnFlash()
    {
        if (!player.CanMove)
        {
            Flash.Action();
            flashCount--;
            if (flashCount <= 0)
            {
                isLastFlash = true;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player.gameObject)
        {
            player.CanMove = false;
            isWorking = true;
            stopPlayerTimer.StartTimer();
            destroyTimer.StartTimer();
            stopTrigger.enabled = false;
        }
    }

    private float auramatT = 0f;
    private float matT = 0f;
    private void Update()
    {
        if (!player.CanMove)
        {
            auramatT += Time.deltaTime * auramatValueChangeSpeed;
            auramat.SetFloat(auramatValueKey, auramatT);
        }

        if (isLastFlash)
        {
            matT += Time.deltaTime * matValueChangeSpeed;
            mat.SetFloat(matValueKey, matT);
        }
    }

    private void OnDestroy()
    {
        auramat.SetFloat(auramatValueKey, 0f);
        mat.SetFloat(matValueKey, 0f);
    }
}
