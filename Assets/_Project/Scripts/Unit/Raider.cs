using System;
using UnityEngine;

public class Raider : MonoBehaviour
{
    [SerializeField] private Animal raider;
    
    [SerializeField] private Gun gun;
    [SerializeField] private Timer shootDelayTimer;
    [SerializeField] private float shootPower;
    
    private Transform target;
    private void Start()
    {
        target = FindObjectsByType<Controller>(FindObjectsSortMode.None)[0].transform;
    }

    private void OnEnable()
    {
        shootDelayTimer.OnTimerEnd += Shoot;
    }

    private void OnDisable()
    {
        shootDelayTimer.OnTimerEnd -= Shoot;
    }

    private void Shoot()
    {
        gun.Shoot(shootPower);
    }
    
    private void Update()
    {
        gun.Target = target;
    }
}
