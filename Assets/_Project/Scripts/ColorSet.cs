using System;
using UnityEngine;

public class ColorSet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private bool autoSet = true;
    [SerializeField] private Color set = Color.white;
    [SerializeField] private float speed = 5f;

    private void Start()
    {
        if (autoSet)
        {
            set = spr.color;
            spr.color = new Color(set.r, set.g, set.b, 0);
        }
    }

    private void Update()
    {
        spr.color = Color.Lerp(spr.color, set, Time.deltaTime * speed);
    }
}
