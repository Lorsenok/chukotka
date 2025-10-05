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
            hp = value;
        }
    }

    private int hp = 0;

    [SerializeField] protected GameObject objectToDestroy;
    [SerializeField] protected int hpSet = 1;
    [SerializeField] protected int maxHP = 1;
    [SerializeField] protected bool saveHP = false;
    [SerializeField] protected string saveHPKey;
    
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
            Destroy(objectToDestroy);
        }
    }
}
