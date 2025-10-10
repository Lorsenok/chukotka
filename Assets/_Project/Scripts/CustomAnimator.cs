using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    public int priority = 0;
    public string animname = "default";
    
    [SerializeField] private SpriteRenderer spr;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float timePerFrame;
    [SerializeField] private bool loop;
    
    private float curTimePerFrame = 0f;
    [SerializeField] private int curSprite = 0;
    
    private void Start()
    {
        spr.sprite = sprites[curSprite];
    }

    public void Reset()
    {
        curSprite = 0;
    }

    private void Update()
    {
        if (!loop && curSprite == sprites.Length - 1) return;
        if (loop && curSprite == sprites.Length - 1) curSprite = 0;
        
        curTimePerFrame -= Time.deltaTime;

        if (curTimePerFrame <= 0f)
        {
            curTimePerFrame = timePerFrame;
            curSprite++;
            spr.sprite = sprites[curSprite];
        }
    }
}