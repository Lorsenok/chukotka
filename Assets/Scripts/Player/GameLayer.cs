using UnityEngine;

public class GameLayer : MonoBehaviour
{
    public bool isWorking = false;
    public bool EnableBackground { get; set; } = true;

    [SerializeField] private GameObject[] objects;
    [SerializeField] private GameObject[] background;

    private void Update()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(isWorking);
        }
        foreach (GameObject obj in background)
        {
            obj.SetActive(EnableBackground);
        }
    }
}
