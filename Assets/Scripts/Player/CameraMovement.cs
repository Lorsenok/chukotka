using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool ylock = true;

    private float starty = 0f;

    private void Start()
    {
        starty = transform.position.y;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, starty, transform.position.z);
    }
}
