using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Controler main;

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
        main.CanJump = colliders.Count > 0;
    }
}
