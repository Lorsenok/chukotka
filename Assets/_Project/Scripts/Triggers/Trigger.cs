using System;
using UnityEngine;
using Zenject;

public class Trigger : MonoBehaviour //Idk how to do it in another way actually
{
    public static Action<int> TriggerById { get; set; }
    public static Action TriggerAll { get; set; }

    [SerializeField] protected int id = -1;
    [SerializeField] protected GameObject[] spawnObjects;
    [SerializeField] protected GameObject[] spawnObjectsPos;
    [SerializeField] protected GameObject[] enableObjects;
    [SerializeField] protected GameObject[] disableObjects;
    [SerializeField] protected GameObject[] destroyObjects;

    [SerializeField] protected int activateAnotherTriggerId = -1;

    public virtual void OnEnable()
    {
        TriggerAll += Action;
        TriggerById += ActionById;
    }

    public virtual void OnDisable()
    {
        TriggerAll -= Action;
        TriggerById -= ActionById;
    }

    public virtual void Action()
    {
        foreach (var obj in spawnObjects)
        {
            Instantiate(obj);
        }

        foreach (var obj in spawnObjectsPos)
        {
            Instantiate(obj, transform.position, obj.transform.rotation);
        }

        foreach (var obj in enableObjects)
        {
            obj.SetActive(true);
        }

        foreach (var obj in disableObjects)
        {
            obj.SetActive(false);
        }

        foreach (var obj in destroyObjects)
        {
            Destroy(obj);
        }

        if (activateAnotherTriggerId != -1) TriggerById?.Invoke(activateAnotherTriggerId);
    }

    public void ActionById(int id)
    {
        if (id == this.id) Action();
    }
}
