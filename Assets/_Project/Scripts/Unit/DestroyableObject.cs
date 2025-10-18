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
            if (saveHP) GameSaver.Save(saveHPKey, value);
            if (value < 0 && hp == 0) return;
            if (hp > value)
            {
                foreach (var obj in spawnAfterDamage)
                {
                    Instantiate(obj, transform.position, obj.transform.rotation);
                }
                
                if (animController != null) animController.PullAnimation(getDamageAnim, getDamageAnimTime);
            }
            hp = value;
            hp = Mathf.Clamp(hp, 0, maxHP);
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
    [SerializeField] protected GameObject[] spawnAfterDestroy;
    
    [Header("Animations")]
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string getDamageAnim;
    [SerializeField] private float getDamageAnimTime;
    
    public virtual void Start()
    {
        if (objectToDestroy == null) objectToDestroy = gameObject;
        hp = hpSet;
        if (PlayerPrefs.HasKey(saveHPKey) && saveHP) hp = (int)GameSaver.Load(saveHPKey, typeof(int));
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
