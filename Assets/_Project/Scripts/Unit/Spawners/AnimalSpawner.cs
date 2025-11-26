using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Prefabs & Tags")] 
    [SerializeField] private GameObject animalPrefab;
    [SerializeField] private string animalTag = "Wolf";

    [Header("Spawn area")]
    [SerializeField] private bool lockToTarget = false;
    [SerializeField] private Transform target;
    [SerializeField] private float maxRange = 20f;
    [SerializeField] private float minRange = 2f;
    [SerializeField] private float startCheckDelay = 1f;

    [Header("Timers")]
    [SerializeField] private float animalCheckInterval = 30f;  
    [SerializeField] private int animalSpawn = 2;          

    private Vector3 startPosition;
    private Coroutine animalRoutine;

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(StartRoutinesDelayed(startCheckDelay));
    }

    private IEnumerator StartRoutinesDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        animalRoutine = StartCoroutine(AnimalChecker());
    }
    private IEnumerator AnimalChecker()
    {
        while (true)
        {
            int current = CountByTag(animalTag);
            if (current < animalSpawn)
            {
                SpawnPrefab(animalPrefab, GetValidSpawnPosition());
            }
            yield return new WaitForSeconds(animalCheckInterval);
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
        if (animalRoutine != null) StopCoroutine(animalRoutine);
    }
}
