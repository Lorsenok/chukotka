using System;
using Unity.VisualScripting;
using UnityEngine;

public class EffectGiver : MonoBehaviour
{
    [SerializeField] private Effect effect;
    [SerializeField] private EffectProperties properties;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Component c = other.gameObject.AddComponent(effect.GetType());
        ((Effect)c).Properties = properties;
    }
}
