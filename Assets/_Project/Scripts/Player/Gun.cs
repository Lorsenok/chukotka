using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Gun : MonoBehaviour
{
    public Transform Target { get; set; }
    [SerializeField] private bool pinToMouse = true;
    
    [SerializeField] private float rotationOffset;
    
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    public void Shoot(float power)
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation).Power = power;
    }
    private void Update()
    {
        Vector2 dir = new Vector2(transform.position.x, transform.position.y) - (pinToMouse ? ProjMath.MousePosition() : Target.position);
        transform.eulerAngles = new Vector3(0f, 0f, ProjMath.RotateTowardsPosition(dir.normalized) + rotationOffset);
    }
}
