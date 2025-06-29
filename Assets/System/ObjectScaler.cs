using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    [SerializeField] private Vector2 defaultRatio;

    private Vector2 startScale;

    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        float curRatio = Screen.width / Screen.height;
        float defRatio = defaultRatio.x / defaultRatio.y;

        transform.localScale = new Vector2(startScale.x, startScale.y) / defRatio * curRatio;
    }
}