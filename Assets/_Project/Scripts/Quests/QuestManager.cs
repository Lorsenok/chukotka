using System.Collections.Generic;
using Zenject;

public class QuestManager
{
    private QuestFactory _factory;
    private QuestInstance _currentQuest;
    
    private List<QuestInstance> _quests;

    [Inject]
    public void Construct(QuestLineConfig config)
    {
        _factory = new QuestFactory(config);
    }

    public void StartQuest()
    {
        _quests = _factory.CreateQuests();
        _currentQuest.Start();
    }

    public void Debug()
    {
        UnityEngine.Debug.Log("sdkjfsdfsdof42");
    }
}

// using UnityEngine;
// using System.Collections.Generic;
//
// public class QuestManager : MonoBehaviour
// {
//     [Header("Configure quest line here")]
//     [SerializeField] private QuestLineConfig questLine;
//
//     private List<QuestInstance> _questInstances;
//     private int _currentQuestIndex = -1;
//
//     private void Awake()
//     {
//         if (questLine == null)
//         {
//             Debug.LogWarning("QuestLine not assigned");
//             return;
//         }
//
//         // Создаём инстансы квестов заранее — можно лениво при старте тоже.
//         _questInstances = new List<QuestInstance>(questLine.quests.Count);
//         foreach (var q in questLine.quests)
//             _questInstances.Add(new QuestInstance(q));
//     }
//
//     private void Start()
//     {
//         StartNextQuest();
//     }
//
//     private void Update()
//     {
//         if (_currentQuestIndex >= 0 && _currentQuestIndex < _questInstances.Count)
//         {
//             var current = _questInstances[_currentQuestIndex];
//             current.Update();
//
//             if (current.IsCompleted)
//                 StartNextQuest();
//         }
//     }
//
//     private void StartNextQuest()
//     {
//         // Завершаем предыдущий (если есть)
//         if (_currentQuestIndex >= 0 && _currentQuestIndex < _questInstances.Count)
//         {
//             _questInstances[_currentQuestIndex].Dispose();
//         }
//
//         _currentQuestIndex++;
//
//         if (_currentQuestIndex < _questInstances.Count)
//         {
//             _questInstances[_currentQuestIndex].Start();
//             Debug.Log($"Quest started: {questLine.quests[_currentQuestIndex].questDescription}");
//         }
//         else
//         {
//             Debug.Log("All quests in the line are completed.");
//         }
//     }
//
//     private void OnDestroy()
//     {
//         if (_questInstances == null) return;
//         foreach (var q in _questInstances)
//             q.Dispose();
//     }
// }