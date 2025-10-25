using System;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public Action OnDie { get; set; }           
    public Action<int> OnHPChanged { get; set; } 

    [Header("HP Settings")]
    [SerializeField] private int hpSet = 100;      
    [SerializeField] private int maxHP = 100;      
    [SerializeField] private bool saveHP = false;  
    [SerializeField] private string saveHPKey;
    [SerializeField] private bool destroyAfterDying = true;
    [SerializeField] private GameObject objectToDestroy;

    [Header("Spawn Objects")]
    [SerializeField] private GameObject[] spawnAfterDamage;
    [SerializeField] private GameObject[] spawnAfterDestroy;

    [Header("Animations")]
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string getDamageAnim;
    [SerializeField] private float getDamageAnimTime;

    private int hp;

    public int HP
    {
        get => hp;
        set
        {
            int newHP = Mathf.Clamp(value, 0, MaxHP);
            int delta = newHP - hp;

            if (delta < 0) 
            {
                foreach (var obj in spawnAfterDamage)
                    Instantiate(obj, transform.position, transform.rotation);

                if (animController != null)
                    animController.PullAnimation(getDamageAnim, getDamageAnimTime);
            }

            hp = newHP;

            if (delta != 0)
                OnHPChanged?.Invoke(hp);

            if (saveHP)
                GameSaver.Save(saveHPKey, hp);

            if (hp <= 0)
                Die();
        }
    }

    public int MaxHP => maxHP;

    private void Awake()
    {
        if (objectToDestroy == null)
            objectToDestroy = gameObject;
    }

    private void Start()
    {
        hp = hpSet;
        if (saveHP && PlayerPrefs.HasKey(saveHPKey))
            hp = (int)GameSaver.Load(saveHPKey, typeof(int));

        hp = Mathf.Clamp(hp, 0, MaxHP);

        OnHPChanged?.Invoke(hp);
    }

    private void Die()
    {
        OnDie?.Invoke();
        enabled = false;

        foreach (var obj in spawnAfterDestroy)
            Instantiate(obj, transform.position, transform.rotation);

        if (destroyAfterDying)
            Destroy(objectToDestroy);
    }
}