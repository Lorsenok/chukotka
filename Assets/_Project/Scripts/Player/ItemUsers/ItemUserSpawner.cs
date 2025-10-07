using UnityEngine;

public class ItemUserSpawner : ItemUser
{
    [Header("Spawn")]
    [SerializeField] private GameObject[] spawnObjects;
    public override void Action()
    {
        foreach (var obj in spawnObjects)
        {
            Instantiate(obj, transform.position, target.rotation);
        }
    }
}
