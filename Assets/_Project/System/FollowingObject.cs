using UnityEngine;

public class FollowingObject : MonoBehaviour
{
    // If you want make object follow other object but without making it children of an object

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        transform.position = target.position + offset;
    }
}
