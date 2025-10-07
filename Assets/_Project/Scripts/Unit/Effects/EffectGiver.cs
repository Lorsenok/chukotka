using System;
using Unity.VisualScripting;
using UnityEngine;

public class EffectGiver : MonoBehaviour
{
    [SerializeField] protected Effect effect;
    [SerializeField] protected EffectProperties properties;
    [SerializeField] protected Timer timerToDestroy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        AddComponent(other.gameObject, effect, properties);
    }

    private void OnDestroyTimerEnd()
    {
        Destroy(gameObject);
    }

    public void OnEnable()
    {
        if (timerToDestroy != null) timerToDestroy.OnTimerEnd += OnDestroyTimerEnd;
    }
    
    public void OnDisable()
    {
        if (timerToDestroy != null) timerToDestroy.OnTimerEnd -= OnDestroyTimerEnd;
    }

    public static Effect AddComponent(GameObject gameObject, Effect effect, EffectProperties properties)
    {
        Component c = gameObject.AddComponent(effect.GetType());
        ((Effect)c).Properties = properties;
        return (Effect)c;
    }
}
