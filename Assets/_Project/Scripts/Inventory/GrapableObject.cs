using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class GrapableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static GrapableObject CurGrappedObject { get; private set; }
    protected static bool canTake = true;

    protected bool isMouseOn = false;

    public Vector2 InitialPosition { get; set; }

    [SerializeField] private float grapSpeed;
    [SerializeField] private Image image;
    [SerializeField] private RectTransform pos;

    protected bool hasTaken = false;

    protected InputSystem inputProvider;

    protected Canvas parentCanvas;

    [Inject]
    private void Init(IInputControler input)
    {
        inputProvider = input.GetInputSystem();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOn = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isMouseOn = false;
    }

    public virtual void OnGrapStart()
    {
    }

    public virtual void OnGrapEnd()
    {
    }

    private bool onGrap = false;

    public virtual void Start()
    {
        pos.localPosition = InitialPosition;
    }

    private Vector2 curPos = Vector2.zero;
    public virtual void Update()
    {
        image.raycastTarget = canTake;

        if (isMouseOn && inputProvider.UI.Click.IsPressed() && canTake)
        {
            if (!onGrap)
            {
                onGrap = true;
                OnGrapStart();
            }

            CurGrappedObject = this;

            hasTaken = true;
            canTake = false;
        }
        else if (!inputProvider.UI.Click.IsPressed())
        {
            if (onGrap)
            {
                onGrap = false;
                OnGrapEnd();
            }

            hasTaken = false;
            canTake = true;
        }

        if (hasTaken)
        {
            if (parentCanvas == null) parentCanvas = GetComponentInParent<Canvas>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition,
                parentCanvas.worldCamera,
                out Vector2 localPoint
            );

            pos.anchoredPosition = Vector2.Lerp(pos.anchoredPosition,
                localPoint, grapSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition,
                InitialPosition, grapSpeed * Time.deltaTime);
        }
    }
}