using System;
using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    [SerializeField] private int progression;
    [SerializeField] private bool doEnableOnProgression = true;
    [SerializeField] private GameObject objToControl;

    private void CheckProgression()
    {
        int curProg = PlayerPrefs.HasKey("progression") ? (int)GameSaver.Load("progression", typeof(int)) : Trigger.Progression;
        
        bool act = curProg == progression
            ? doEnableOnProgression
            : !doEnableOnProgression;
            
        objToControl.SetActive(act);
    }
    
    private void Awake()
    {
        CheckProgression();
        Trigger.OnProgressionChanged += CheckProgression;
    }

    private void OnDestroy()
    {
        Trigger.OnProgressionChanged -= CheckProgression;
    }
}
