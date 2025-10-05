using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class GroundChecker : MonoBehaviour
{
    public bool IsTouchingGround { get; set; } = false;

    private List<Collider2D> colliders = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ground>())
        {
            colliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Ground>())
        {
            colliders.Remove(collision);
        }
    }

    private void FixedUpdate()
    {
        IsTouchingGround = colliders.Count > 0;
    }
}
