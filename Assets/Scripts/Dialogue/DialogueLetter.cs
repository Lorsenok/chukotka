using TMPro;
using UnityEngine;

public class DialogueLetter : MonoBehaviour
{
    [SerializeField] private Color curColor = Color.white;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private bool colorSmoothSet = false;

    [SerializeField] private float effectPower = 5f;

    [SerializeField] private float rotationSpeedM = 1f;
    [SerializeField] private float shakeSpeedM = 0.1f;
    [SerializeField] private Vector3 shakeOffset;

    [SerializeField] private float timeBeforeEffect = 0f;

    private float rotationSpeed = 0f;
    private float shakePower = 0f;

    private Vector3 startPosition;
    private Vector3 curPosition;

    public void ApplyEffect(string effect)
    {
        switch (effect)
        {
            case "r":
                curColor = Color.red; shakePower = effectPower * shakeSpeedM; break;

            case "g":
                curColor = Color.green; break;

            case "b":
                rotationSpeed = effectPower * rotationSpeedM; curColor = Color.blue; break;

            case "a":
                curColor = new Color(curColor.r, curColor.g, curColor.b, 0f); break;
        }
    }

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        if (timeBeforeEffect > 0f)
        {
            timeBeforeEffect -= Time.deltaTime;
            return;
        }

        text.color = colorSmoothSet ? Color.Lerp(text.color, curColor, effectPower * Time.deltaTime) : curColor;

        transform.eulerAngles += new Vector3(0f, 0f, rotationSpeed * Time.deltaTime);
        curPosition = startPosition + new Vector3(Random.Range(-shakePower, shakePower), 
            Random.Range(-shakePower, shakePower), 
            Random.Range(-shakePower, shakePower)) + shakeOffset;
        transform.localPosition = Vector3.Lerp(transform.localPosition, curPosition, Time.deltaTime * shakePower);
    }
}
