using System;
using UnityEngine;

public class CustomAnimatorController : MonoBehaviour
{
    [SerializeField] private CustomAnimator[] animators;
    private float[] animatorTimers;
    
    public void PullAnimation(string name, float time)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i].animname == name)
            {
                animatorTimers[i] = time;
            }
        }
    }

    public void ResetAnimation()
    {
        curanim.Reset();
    }

    public void ResetController()
    {
        Awake();
        curanim = null;
    }

    private void Awake()
    {
        animatorTimers = new float[animators.Length];
        for (int i = 0; i < animators.Length; i++) animatorTimers[i] = 0f;
    }

    private CustomAnimator curanim;
    private int lastAnimId = 0;
    private void Update()
    {
        if (!curanim) curanim = animators[0];
        
        for (int i = 0; i < animators.Length; i++)
        {
            animatorTimers[i] -= Time.deltaTime;
            if (animatorTimers[i] > 0f && animators[i].priority > curanim.priority | animatorTimers[lastAnimId] <= 0f)
            {
                lastAnimId = i;
                curanim = animators[i];
            }
        }
        
        foreach (var anim in animators)
        {
            if (anim != curanim)
            {
                anim.enabled = false;
                anim.Reset();
            }
        }
        curanim.enabled = true;
    }
}
