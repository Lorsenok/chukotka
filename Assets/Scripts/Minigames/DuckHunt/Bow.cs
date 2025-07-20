using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Bow : MonoBehaviour
{
    [SerializeField] private float rotationOffset;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float arrowHoldSpeed;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform arrowIdlePoint;
    [SerializeField] private Transform arrowHoldPoint;

    [SerializeField] private Timer shootDelay;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private InputSystem inputSystem;
    [Inject] private void Init(IInputControler input)
    {
        inputSystem = input.GetInputSystem();
    }

    private bool isHolding = false;
    private void OnClick(InputAction.CallbackContext context)
    {
        isHolding = !isHolding;

        if (isHolding || !canShoot) return;

        canShoot = false;
        shootDelay.StartTimer();
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation);
    }

    private bool canShoot = true;
    private void OnShootDelayEnd()
    {
        canShoot = true;
    }

    private void OnEnable()
    {
        inputSystem.UI.Click.performed += OnClick;
        shootDelay.OnTimerEnd += OnShootDelayEnd;
    }

    private void OnDisable()
    {
        inputSystem.UI.Click.performed -= OnClick;
        shootDelay.OnTimerEnd -= OnShootDelayEnd;
    }

    Vector3 curRotation = Vector3.zero;
    private void Update()
    {
        arrow.localPosition = 
            Vector3.Lerp(arrow.localPosition, 
            isHolding ? arrowHoldPoint.localPosition : arrowIdlePoint.localPosition, Time.deltaTime * arrowHoldSpeed);
         
        Vector2 dir = new Vector2(transform.position.x, transform.position.y) - ProjMath.MousePosition();
        curRotation = new Vector3(0f, 0f, ProjMath.RotateTowardsPosition(dir.normalized) + rotationOffset);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(curRotation), rotationSpeed * Time.deltaTime);
    }
}
