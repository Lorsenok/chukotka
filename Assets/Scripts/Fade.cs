using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Action OnFadeEnd { get; set; }

    [SerializeField] private Image image;
    [SerializeField] private float speed = 0.7f;

    private float opacityTarget = 0;

    public void StartFade()
    {
        opacityTarget = 1f;
    }

    private void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, opacityTarget, speed * Time.deltaTime));

        if (image.color.a >= 1f && opacityTarget == 1f)
        {
            opacityTarget = 0f;
            OnFadeEnd?.Invoke();
        }
    }
}
