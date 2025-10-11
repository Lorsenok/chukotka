using System;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Image img;
    
    private static float curFlashTime = 0f;
    public static void Action()
    {
        curFlashTime = 1f;
    }

    private void Update()
    {
        if (curFlashTime > 0f) curFlashTime -= Time.deltaTime * speed;
        img.color = new Color(1f, 1f, 1f, curFlashTime);
    }
}
