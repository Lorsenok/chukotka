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
        if (animController != null) animController.PullAnimation(shootAnim, shootAnimTime);
    }
    private void Update()
    {
        if (Target == null && !pinToMouse) return;
        
        Vector2 dir = new Vector2(transform.position.x, transform.position.y) - (pinToMouse ? ProjMath.MousePosition() : Target.position);
        float rotation = ProjMath.RotateTowardsPosition(dir.normalized);
        
        Vector3 targetPos = Target == null ? ProjMath.MousePosition() : Target.position;
        
        if (rotationLocks.Length > 0)
            rotation = FindClosestRotation(rotation);
        transform.eulerAngles = new Vector3(0f, 0f, rotation + rotationOffset + (targetPos.x > transform.position.x ? rotationOffsetByX : -rotationOffsetByX));
    }
}
