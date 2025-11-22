using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    [Header("Progression Settings")]
    [SerializeField] private int targetProgression;
    [SerializeField] private bool unlockForever;
    [SerializeField] private bool doEnableOnProgression = true;

    [Header("Object To Control")]
    [SerializeField] private GameObject objToControl;
    
    private void Awake()
    {
        CheckProgression();
        Trigger.OnProgressionChanged += CheckProgression;
    }

    private void OnDestroy()
    {
        Trigger.OnProgressionChanged -= CheckProgression;
    }
    
    private void CheckProgression()
    {
        int current = GetCurrentProgression();
        bool active = ShouldBeActive(current);
        objToControl.SetActive(active);
    }
    
    private int GetCurrentProgression()
    {
        if (PlayerPrefs.HasKey("progression"))
            return (int)GameSaver.Load("progression", typeof(int));

        return Trigger.Progression;
    }
    
    private bool ShouldBeActive(int current)
    {
        if (unlockForever)
            return current >= targetProgression ? doEnableOnProgression : !doEnableOnProgression;

        return current == targetProgression ? doEnableOnProgression : !doEnableOnProgression;
    }
}