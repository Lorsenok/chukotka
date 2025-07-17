using System;
using UnityEngine;
using Zenject;

public class Trigger : MonoBehaviour //Idk how to do it in another way actually
{
    public static Action<int> TriggerById { get; set; }
    public static Action TriggerAll { get; set; }

    [SerializeField] private int id = -1;
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private GameObject[] spawnObjectsPos;
    [SerializeField] private GameObject[] enableObjects;
    [SerializeField] private GameObject[] disableObjects;
    [SerializeField] private GameObject[] destroyObjects;

    [SerializeField] private int activateAnotherTriggerId = -1;

    private void OnEnable()
    {
        TriggerAll += Action;
        TriggerById += ActionById;
    }

    private void OnDisable()
    {
        TriggerAll -= Action;
        TriggerById -= ActionById;
    }

    public void Action()
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
