using System.Collections.Generic;
using Zenject;

public class QuestManager : IInitializable
{
    private QuestFactory _factory;
    private QuestInstance _currentQuest;
    
    private List<QuestInstance> _quests;

    [Inject]
    public void Construct(QuestLineConfig config, QuestDialogEvents dialogEvents, 
        IInventory inventory, IAbilityContainer abilityContainer)
    {
        _factory = new QuestFactory(config, dialogEvents, inventory, abilityContainer);
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
        _currentQuest.Start();
        _currentQuest.OnCompleted += OnQuestCompleted;
    }

    private void OnQuestCompleted()
    {
        _currentQuest.OnCompleted -= OnQuestCompleted;
        _currentQuest.Dispose();
        _quests.RemoveAt(0);
        StartQuest();
    }
}