using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Prefabs & Tags")]
    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private string birdTag = "Bird"; 
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private string wolfTag = "Wolf";

    [Header("Spawn area")]
    [SerializeField] private bool lockToTarget = false;
    [SerializeField] private Transform target;
    [SerializeField] private float maxRange = 20f;
    [SerializeField] private float minRange = 2f;
    [SerializeField] private float startCheckDelay = 1f;

    [Header("Timers")]
    [SerializeField] private float birdCheckInterval = 30f;  
    [SerializeField] private int birdSpawn = 6;          
    [SerializeField] private int birdSpawnMin = 1;           
    [SerializeField] private int birdSpawnMax = 3;

    [SerializeField] private float wolfCheckInterval = 30f;  
    [SerializeField] private int wolfSpawn = 2;          

    private Vector3 startPosition;
    private Coroutine birdRoutine;
    private Coroutine wolfRoutine;

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(StartRoutinesDelayed(startCheckDelay));
    }

    private IEnumerator StartRoutinesDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        birdRoutine = StartCoroutine(BirdChecker());
        wolfRoutine = StartCoroutine(WolfChecker());
    }

    private IEnumerator BirdChecker()
    {
        while (true)
        {
            int current = CountByTag(birdTag);
            if (current < birdSpawn)
            {
                int toSpawn = Random.Range(birdSpawnMin, birdSpawnMax + 1);
                for (int i = 0; i < toSpawn; i++)
                {
                    Vector3 pos = GetValidSpawnPosition();
                    SpawnPrefab(birdPrefab, pos);
                }
            }
            yield return new WaitForSeconds(birdCheckInterval);
        }
    }

    private IEnumerator WolfChecker()
    {
        while (true)
        {
            int current = CountByTag(wolfTag);
            if (current < wolfSpawn)
            {
                SpawnPrefab(wolfPrefab, GetValidSpawnPosition());
            }
            yield return new WaitForSeconds(wolfCheckInterval);
        }
    }

    private int CountByTag(string tag)
    {
        try
        {
            var objs = GameObject.FindGameObjectsWithTag(tag);
            return objs.Length;
        }
        catch
        {
            return 0;
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        float curRange = 0f;
        int maxIter = 100;
        Vector3 candidate = startPosition;

        while (maxIter-- > 0)
        {
            curRange = Random.Range(-maxRange, maxRange);

            if (Mathf.Abs(curRange) < minRange) continue;

            float x = startPosition.x + curRange;
            candidate = new Vector3(x, startPosition.y, startPosition.z);
            return candidate;
        }

        return startPosition;
    }

    private void SpawnPrefab(GameObject prefab, Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);
    }

    private void Update()
    {
        if (!lockToTarget) return;

        if (target == null)
        {
            var controllers = FindObjectsByType<Controller>(FindObjectsSortMode.None);
            if (controllers.Length > 0)
                target = controllers[0].transform;
            else
                return;
        }

        startPosition = target.position;
    }

    private void OnDisable()
    {
        if (birdRoutine != null) StopCoroutine(birdRoutine);
        if (wolfRoutine != null) StopCoroutine(wolfRoutine);
    }

    public void OnPositionChange()
    {
        startPosition = GetValidSpawnPosition();
        transform.position = startPosition;
    }
}
