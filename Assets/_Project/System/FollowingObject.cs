using System;
using UnityEngine;

public class FollowingObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool targetIsCamera;

    private void Start()
    {
        if (targetIsCamera) target = Camera.main.transform;
    }

    private void Update()
    {
        transform.position = target.position + offset;
    }
}
