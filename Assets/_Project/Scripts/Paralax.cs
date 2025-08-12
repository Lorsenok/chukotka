using UnityEngine;

public class ParalaxMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Transform target;

    private float startX = 0f;

    private void Start()
    {
        startX = spr.size.x;
    }

    private void Update()
    {
        float dirX = transform.position.x - target.position.x;
        spr.size = new Vector2(startX + dirX * speed, spr.size.y);
    }
}
