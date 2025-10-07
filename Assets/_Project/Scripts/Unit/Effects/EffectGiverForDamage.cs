using UnityEngine;

public class EffectGiverForDamage : EffectGiver
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ControllerAttack>() != null)
        {
            foreach (DamageObject obj in other.GetComponentsInChildren<DamageObject>())
            {
                obj.CurEffect = AddComponent(obj.gameObject, effect, properties);
                obj.CurEffect.enabled = false;
                obj.EffectProps = properties;
                Destroy(gameObject);
            }
        }
    }
}
