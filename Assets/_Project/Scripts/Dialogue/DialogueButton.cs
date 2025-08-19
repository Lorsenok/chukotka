using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : GameButton
{
    public int Id { get; set; }
    public static Action<int> OnButtonPressed { get; set; }

    [SerializeField] private Timer blockTimer;

    [Header("Question Mark")]
    [SerializeField] private float questionMarkSpeed = 5f;

    [SerializeField] private RectTransform questionMark;
    [SerializeField] private Vector3 questionMarkScale = Vector3.one;

    [SerializeField] private Image questionMarkImage;
    [SerializeField] private Color questionMarkDefaultColor;
    [SerializeField] private Color questionMarkSelectedColor;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float textColorChangeSpeed;
    [SerializeField] private Color defaultTextColor = Color.white;
    [SerializeField] private Color selectedTextColor;

    private bool block = true;

    private Vector3 startScale;

    public override void OnEnable()
    {
        base.OnEnable();
        blockTimer.OnTimerEnd += OnBlockEnd;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        blockTimer.OnTimerEnd -= OnBlockEnd;
    }

    private void OnBlockEnd()
    {
        block = false;
    }

    public override void Start()
    {
        base.Start();
        startScale = questionMark.localScale;
    }

    public void Update()
    {
        questionMark.localScale = 
            Vector3.Lerp(questionMark.localScale, 
            isMouseOn ? questionMarkScale : startScale, 
            Time.deltaTime * questionMarkSpeed);

        questionMarkImage.color =
            Color.Lerp(questionMarkImage.color,
            isMouseOn ? questionMarkSelectedColor : questionMarkDefaultColor,
            Time.deltaTime * questionMarkSpeed);

        text.color =
            Color.Lerp(text.color,
            isMouseOn ? selectedTextColor : defaultTextColor,
            Time.deltaTime * textColorChangeSpeed);
    }

    public override void Action()
    {
        if (block) return;

        base.Action();
        if (isMouseOn)
        {
            OnButtonPressed?.Invoke(Id);
        }
    }
}
