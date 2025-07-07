using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLayerSwitcher : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float speed = 0.7f;

    public static Action<int> OnGameLayerSwitch { get; set; }

    private float opacityTarget = 0;
    private int newLayerId = 0;

    private void OnEnable()
    {
        GameLayerChangerService.OnLayerChange += ChangeLayer;
    }

    private void OnDisable()
    {
        GameLayerChangerService.OnLayerChange -= ChangeLayer;
    }

    public void ChangeLayer(int id)
    {
        newLayerId = id;
        opacityTarget = 1;
    }

    private void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, opacityTarget, speed * Time.deltaTime));

        if (image.color.a >= 1)
        {
            opacityTarget = 0;
            OnGameLayerSwitch?.Invoke(newLayerId);
        }
    }
}
