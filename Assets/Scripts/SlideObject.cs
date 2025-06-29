using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Zenject;

public class SlideObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform moveTransform;
    private Vector3 moveTransformStartPosition;
    [SerializeField] private bool yFollow = true;
    [SerializeField] private bool xFollow = false;
    [SerializeField] private float multiplier = 1f;

    protected InputSystem inputSystem;
    [Inject]
    public void Init(IInputControler controler)
    {
        inputSystem = controler.GetInputSystem();
    }

    private bool isMouseOn = false;

    private void OnMouseEnter()
    {
        isMouseOn = true;
    }

    private void OnMouseExit()
    {
        isMouseOn = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    public virtual void OnEnable()
    {
        inputSystem.UI.Click.performed += OnClick;
    }

    public virtual void OnDisable()
    {
        inputSystem.UI.Click.performed -= OnClick;
    }

    protected bool isHolding = false;
    public virtual void OnClick(InputAction.CallbackContext context)
    {
        isHolding = !isHolding;
        if (isHolding && isMouseOn)
        {
            startMousePosition = Input.mousePosition;
            startPosition = moveTransform.localPosition;
        }
    }

    private Vector3 startMousePosition;
    private Vector3 startPosition;

    private void Update()
    {
        if (isHolding && isMouseOn)
        {
            Vector3 offset = (Input.mousePosition - startMousePosition) * multiplier;
            moveTransform.localPosition = startPosition + offset;
            moveTransform.localPosition = new Vector3(xFollow ? moveTransform.localPosition.x : startPosition.x,
                yFollow ? moveTransform.localPosition.y : startPosition.y, moveTransform.localPosition.z);
        }
    }

    private void Awake()
    {
        moveTransformStartPosition = moveTransform.localPosition;
    }

    public void ResetPosition()
    {
        moveTransform.localPosition = moveTransformStartPosition;
    }
}
