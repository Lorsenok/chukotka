using System;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject[] spawnOnDamage;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float shakeOnDamage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent(out DestroyableObject obj);
        if (obj == null) return;
        obj.HP -= damage;
        foreach (GameObject spawn in spawnOnDamage)
        {
            Instantiate(spawn, spawnPoint.position, spawn.transform.rotation);
        }
        CameraMovement.Shake(shakeOnDamage);
    }
}
