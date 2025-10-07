using System;
using Unity.VisualScripting;
using UnityEngine;

public class EffectGiver : MonoBehaviour
{
    [SerializeField] protected Effect effect;
    [SerializeField] protected EffectProperties properties;

    private void OnTriggerEnter2D(Collider2D other)
    {
        AddComponent(other.gameObject, effect, properties);
    }

    public static Effect AddComponent(GameObject gameObject, Effect effect, EffectProperties properties)
    {
        Component c = gameObject.AddComponent(effect.GetType());
        ((Effect)c).Properties = properties;
        return (Effect)c;
    }
}
