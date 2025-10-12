using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Gun : MonoBehaviour
{
    public Transform Target { get; set; }
    [SerializeField] private bool pinToMouse = true;
    
    [SerializeField] private float rotationOffset;
    [SerializeField] private float rotationOffsetByX;
    [SerializeField] private float[] rotationLocks;
    
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    
    [Header("Animation")]
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string shootAnim;
    [SerializeField] private float shootAnimTime;

    private float FindClosestRotation(float rotation) //by chatgpt
    {
        float closest = rotationLocks[0];
        float minDiff = 360f;

        foreach (float lockAngle in rotationLocks)
        {
            float diff = Mathf.Abs(Mathf.DeltaAngle(rotation, lockAngle));

            if (diff < minDiff)
            {
                minDiff = diff;
                closest = lockAngle;
            }
        }

        return closest;
    }

    
    public void Shoot(float power)
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation).Power = power;
        animController.PullAnimation(shootAnim, shootAnimTime);
    }
    private void Update()
    {
        if (Target == null) return;
        
        Vector2 dir = new Vector2(transform.position.x, transform.position.y) - (pinToMouse ? ProjMath.MousePosition() : Target.position);
        float rotation = ProjMath.RotateTowardsPosition(dir.normalized);
        if (rotationLocks.Length > 0)
            rotation = FindClosestRotation(rotation);
        transform.eulerAngles = new Vector3(0f, 0f, rotation + rotationOffset + (Target.position.x > transform.position.x ? rotationOffsetByX : -rotationOffsetByX));
    }
}
