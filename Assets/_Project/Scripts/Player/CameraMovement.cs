using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool ylock = true;
    [SerializeField] private GameState stateLock = GameState.Game;

    private IGameState state;
    [Inject] private void Init(IGameState state)
    {
        this.state = state;
    }

    private float starty = 0f;

    private void Start()
    {
        starty = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (state.GetCurrectState() != stateLock) return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, ylock ? starty : transform.position.y, transform.position.z);
    }
}
