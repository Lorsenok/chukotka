using UnityEngine;

public class DamagebleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageble damageble))
        {
            Destroy(collision.gameObject);
        }
    }
}
