using UnityEngine;

public class DuckSpawner : MonoBehaviour
{
    [SerializeField] private Timer spawnDelay;
    [SerializeField] private Duck prefab;
    [SerializeField] private Vector3 direction;

    private void OnEnable()
    {
        spawnDelay.OnTimerEnd += Spawn;
    }

    private void OnDisable()
    {
        spawnDelay.OnTimerEnd -= Spawn;
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, transform.rotation).direction = direction;
        spawnDelay.StartTimer();
    }
}
