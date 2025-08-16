using UnityEngine;

public class ObjectScaler : MonoBehaviour // Generated with chatGPT (needs to be replaced!)
{
    // Исходное разрешение (16:9)
    private const float ReferenceAspect = 16f / 9f;

    private Vector3 originalScale;

    private void Awake()
    {
        // Сохраняем исходный масштаб
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Получаем текущее разрешение экрана
        float currentAspect = (float)Screen.width / Screen.height;

        // Вычисляем коэффициент масштабирования
        float scaleFactor = currentAspect / ReferenceAspect;

        // Применяем масштабирование по X и Y (Z оставляем как есть)
        transform.localScale = new Vector3(
            originalScale.x * scaleFactor,
            originalScale.y * scaleFactor,
            originalScale.z
        );
    }
}
