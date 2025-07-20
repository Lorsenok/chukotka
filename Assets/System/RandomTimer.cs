using UnityEngine;

public class RandomTimer : Timer
{
    [SerializeField] private float timeSetMax;

    public override void StartTimer()
    {
        curTime = Random.Range(timeSet, timeSetMax);
    }
}
