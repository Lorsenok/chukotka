using System;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
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
    
    [SerializeField] protected int hpSet = 1;
    [SerializeField] protected int maxHP = 1;
    [SerializeField] protected bool saveHP = false;
    [SerializeField] protected string saveHPKey;
    
    public virtual void Start()
    {
        hp = hpSet;
        if (PlayerPrefs.HasKey(saveHPKey) && saveHP) hp = PlayerPrefs.GetInt(saveHPKey);
    }

    public virtual void Update()
    {
        hp = Mathf.Clamp(hp, 0, maxHP);
        if (hp <= 0) Destroy(gameObject);
    }
}
