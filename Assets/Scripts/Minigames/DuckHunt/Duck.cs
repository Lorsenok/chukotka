using System;
using UnityEngine;

public interface IDamageble
{
    void GetDamage(float damage);
    void Die();
}

public class Duck : MonoBehaviour, IDamageble
{
    public static Action OnDie { get; set; }

    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private float gravity;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlesPrefab;
    public Vector3 direction;

    private void OnEnable()
    {
        Minigame.OnMinigameEnd += OnGameEnd;
    }

    private void OnDestroy()
    {
        Minigame.OnMinigameEnd -= OnGameEnd;
    }

    private void OnGameEnd()
    {
        Destroy(gameObject);
    }

    public void GetDamage(float damage)
    {
        OnCollisionEnter2D(null);
        OnDie?.Invoke();
        rg.gravityScale = gravity;
        enabled = false;
    }

    public void Die()
    {
        GetDamage(0f);
    }

    private void Update()
    {
        spr.flipX = direction.x > 0f;
        transform.position += direction * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(particlesPrefab, transform.position, Quaternion.identity);
    }
}
