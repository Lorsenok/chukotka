using System;
using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    [SerializeField] private float amplitudeMin;
    [SerializeField] private float amplitudeMax;
    [SerializeField] private float speed;
    [SerializeField] private Timer switchTimer;

    private float side = 1f;

    private void SwitchSide()
    {
        side = -side;
    }

    public void OnEnable()
    {
        switchTimer.OnTimerEnd += SwitchSide;
    }

    public void OnDisable()
    {
        switchTimer.OnTimerEnd -= SwitchSide;
    }

    private void Update()
    {
        float curRot = transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 
            curRot + speed * Time.deltaTime * side);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 
            Mathf.Clamp(curRot, amplitudeMin, amplitudeMax));
    }
}
