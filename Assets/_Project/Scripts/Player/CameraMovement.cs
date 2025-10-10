using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool ylock = true;
    [SerializeField] private GameState stateLock = GameState.Game;
    [SerializeField] private float shakeExpireSpeed;
    
    private IGameState state;
    [Inject] private void Init(IGameState state)
    {
        this.state = state;
    }

    private static Vector3 curShakeOffset;
    public static void Shake(float power, bool z = false)
    {
        power *= Config.ShakePower;
        curShakeOffset = new Vector3(
            Random.Range(-power, power),
            Random.Range(-power, power),
            z ?  Random.Range(-power, power) : 0
            );
    }

    private float starty = 0f;

    private void Start()
    {
        starty = transform.position.y;
        Vector3 targetPos = target.position + offset;
        targetPos.y = ylock ? starty : transform.position.y;
        transform.position = targetPos;
    }

    private void FixedUpdate()
    {
        curShakeOffset = Vector3.Lerp(curShakeOffset, Vector3.zero, shakeExpireSpeed * Time.deltaTime);
        if (state.GetCurrentState() != stateLock) return;
        Vector3 targetPos = target.position + offset;
        targetPos.y = ylock ? starty : transform.position.y;
        transform.position = Vector3.Lerp(transform.position, targetPos + curShakeOffset, speed * Time.deltaTime);
    }
}
