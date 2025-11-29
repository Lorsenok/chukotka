using System.Collections.Generic;
using Zenject;

public class QuestManager : IInitializable
{
    private QuestFactory _factory;
    private QuestInstance _currentQuest;
    
    private List<QuestInstance> _quests;

    [Inject]
    public void Construct(QuestLineConfig config, QuestDialogEvents dialogEvents, 
        IInventory inventory, IAbilityContainer abilityContainer, QuestObjectsGlobalRegistry registry)
    {
        _factory = new QuestFactory(config, dialogEvents, inventory, abilityContainer, registry);
    }

    public void Initialize()
    {
        _quests = _factory.CreateQuests();
        StartQuest();
    }
    
    private void StartQuest()
    {
        if (_quests.Count == 0)
            return;
        
        _currentQuest = _quests[0];
        _currentQuest.OnCompleted += OnQuestCompleted;
        _currentQuest.Start();
    }

    private void OnQuestCompleted()
    {
        _currentQuest.OnCompleted -= OnQuestCompleted;
        _currentQuest.Dispose();
        _quests.RemoveAt(0);
        StartQuest();
    }
}