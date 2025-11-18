using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private DestroyableObject destroyableObject;
    [SerializeField] private float speed = 5f;

    private float targetFill;
    private bool instantUpdateDone = false;

    private void OnEnable()
    {
        destroyableObject.OnHPChanged += OnHPChanged;
    }

    private void OnDisable()
    {
        destroyableObject.OnHPChanged -= OnHPChanged;
    }

    private void OnHPChanged(int currentHP)
    {
        targetFill = (float)currentHP / destroyableObject.MaxHP;

        // Если полоска ещё не была установлена при старте сцены — сразу ставим
        if (!instantUpdateDone)
        {
            image.fillAmount = targetFill;
            instantUpdateDone = true;
        }
    }

    private void Update()
    {
        // Плавное изменение только после старта
        if (instantUpdateDone)
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, targetFill, speed * Time.deltaTime);
        }
    }
}