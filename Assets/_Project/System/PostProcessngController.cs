using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessngController : MonoBehaviour
{
    public List<Volume> Volumes { get; set; } = new List<Volume>();

    [SerializeField] private Volume[] startVolumes;
    [SerializeField] private float speed;
    [SerializeField] private float minimumWeight = 0.01f;

    private int id = 0;

    public void Set(Volume volume)
    {
        for (int i = 0; i < Volumes.Count; i++)
        {
            if (Volumes[i] == volume) id = i;
        }
    }

    public void Set(int _id)
    {
        id = _id;
    }

    public Volume AddVolume(Volume volume)
    {
        Volume v = this.AddComponent<Volume>();
        v.profile = volume.profile;

        return v;
    }

    private void Awake()
    {
        foreach (Volume v in startVolumes)
        {
            Volumes.Add(v);
        }
    }

    private void Update()
    {
        for (int i = 0; i < Volumes.Count; i++)
        {
            Volumes[i].weight = Mathf.Lerp(Volumes[i].weight, i == id ? Config.PostProcessing : 0f, speed * Time.deltaTime);
            Volumes[i].weight = Mathf.Clamp(Volumes[i].weight, minimumWeight, 1f);
        }
    }
}
