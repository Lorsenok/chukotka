using System;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    public static float MaxDistance { get; set; } = 100f;
    public static float MinDistance { get; set; } = 0f;
    
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;
    
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    private void Update()
    {
        MaxDistance = maxDistance;
        MinDistance = minDistance;
        
        leftBorder.position = new Vector3(MinDistance, transform.position.y, transform.position.z);
        rightBorder.position = new Vector3(MaxDistance, transform.position.y, transform.position.z);
    }
}
