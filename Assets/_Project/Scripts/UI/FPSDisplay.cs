using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private const int NormalFrameRate = 50;
    private const string FPSPrefix = "FPS: ";
    
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private float updateInterval = 0.2f;
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color lowFpsColor = Color.red;

    private float _accumulatedTime;
    private int _frames;
    private float _timer;

    // Здесь мы не рассчитываем FPS каждый кадр — это шумно.
    // Мы собираем данные и обновляем вывод раз в updateInterval.
    private void Update()
    {
        _accumulatedTime += Time.unscaledDeltaTime;
        _frames++;
        _timer += Time.unscaledDeltaTime;

        if (_timer >= updateInterval)
        {
            int fps = Mathf.RoundToInt(_frames / _accumulatedTime);
            
            fpsText.text = FPSPrefix + fps;
            fpsText.color = CalculateColor(fps);

            _timer = 0f;
            _frames = 0;
            _accumulatedTime = 0f;
        }
    }
    
    private Color CalculateColor(float fps)
    {
        return fps < NormalFrameRate ? lowFpsColor : normalColor;
    }
}