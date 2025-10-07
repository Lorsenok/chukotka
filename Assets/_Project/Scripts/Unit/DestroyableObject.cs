using System;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public Action OnDie { get; set; }
    
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (saveHP) PlayerPrefs.SetInt(saveHPKey, hp);
            foreach (var obj in spawnAfterDamage)
            {
                Instantiate(obj, transform.position, obj.transform.rotation);
            }
            hp = value;
        }
    }

    [SerializeField] private int hp = 0;

    [SerializeField] protected bool destroyAfterDying = true;
    [SerializeField] protected GameObject objectToDestroy;
    [SerializeField] protected int hpSet = 1;
    [SerializeField] protected int maxHP = 1;
    [SerializeField] protected bool saveHP = false;
    [SerializeField] protected string saveHPKey;
    [SerializeField] protected GameObject[] spawnAfterDamage;
    
    public virtual void Start()
    {
        if (objectToDestroy == null) objectToDestroy = gameObject;
        hp = hpSet;
        if (PlayerPrefs.HasKey(saveHPKey) && saveHP) hp = PlayerPrefs.GetInt(saveHPKey);
    }

    public virtual void Update()
    {
        hp = Mathf.Clamp(hp, 0, maxHP);
        if (hp <= 0)
        {
            OnDie?.Invoke();
            enabled = false;
            if (destroyAfterDying) Destroy(objectToDestroy);
        }
    }
}
