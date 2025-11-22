using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int _targetFramerate = 60;
    
    void Start()
    {
        Application.targetFrameRate = _targetFramerate;
    }
}
