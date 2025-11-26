using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdsController : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private bool[] freePoints;
    [SerializeField] private int maxBirds;
    [SerializeField] private Timer birdsSpawnTimer;
    [SerializeField] private GameObject birdPrefab;

    [SerializeField] private Transform viewPoint;
    [SerializeField] private float minDistanceToViewPoint;

    private List<Bird> curBirds = new List<Bird>();

    private int RandomPos(bool checkDistance = false)
    {
        int p = Random.Range(0, points.Length);
        int times = 20;
        while (!freePoints[p])
        {
            p = Random.Range(0, points.Length); // to make sure there will not be more than 1 bird in the same place
            
            times--;
            
            if (
                checkDistance &&
                Vector2.Distance(points[p].position, viewPoint.position) <= minDistanceToViewPoint
            ) continue;
            
            if (times != 0)
            {
                Debug.LogWarning("Couldn't get other random position");
                return -1;
            }
        }
        return p;
    }
    
    private void Spawn()
    {
        if (!freePoints.Contains(true) || curBirds.Count >= maxBirds) return;

        int p = RandomPos(true);
        if (p == -1)
        {
            Debug.LogError("There are no place to spawn a bird!");
            return;
        }
        
        Instantiate(birdPrefab, points[p].position, birdPrefab.transform.rotation)
            .TryGetComponent<Bird>(out var bird);
        if (!bird)
        {
            Debug.LogError("Tried to spawn a bird without a bird component!");
            return;
        }
        
        bird.CurPoint = points[p];
        bird.Controller = this;
        freePoints[p] = false;
        curBirds.Add(bird);
    }

    public void Switch(Bird bird)
    {
        int cur = Array.IndexOf(points, bird.CurPoint);
        freePoints[cur] = true;
        
        int p = RandomPos();
        if (p == -1) p = cur;
        else while (p == cur && p != -1) p = RandomPos();
        
        freePoints[p] = false;
        bird.CurPoint = points[p];
    }
    
    private void Awake()
    {
        freePoints = new bool[points.Length];
        for (int i = 0; i < freePoints.Length; i++) freePoints[i] = true;
    }

    private void Update()
    {
        for (int i = 0; i < curBirds.Count; i++)
        {
            if (curBirds[i]) return;
            
            freePoints[Array.IndexOf(points, curBirds[i].CurPoint)] = true;
            curBirds.Remove(curBirds[i]);
            break;
        }
    }

    private void OnEnable()
    {
        birdsSpawnTimer.OnTimerEnd += Spawn;
    }

    private void OnDisable()
    {
        birdsSpawnTimer.OnTimerEnd -= Spawn;
    }
}
