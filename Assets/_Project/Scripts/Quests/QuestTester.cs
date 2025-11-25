using System;
using UnityEngine;
using Zenject;

public class QuestTester : MonoBehaviour
{
    [Inject]
    public QuestManager QuestManager;
    
    private void Start()
    {
        QuestManager.Debug();
    }
}
