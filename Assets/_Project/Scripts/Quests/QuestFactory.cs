using System.Collections.Generic;
using Zenject;

public class QuestFactory
{
    private readonly QuestLineConfig _config;

    private readonly QuestDialogEvents _dialogEvents;
    
    private readonly IInventory _inventory;
    
    public QuestFactory(QuestLineConfig config, QuestDialogEvents questDialogEvents, IInventory inventory)
    {
        _config = config;
        _dialogEvents = questDialogEvents;
        _inventory = inventory;
    }
    
    public List<QuestInstance> CreateQuests()
    {
        List<QuestInstance> quests = new List<QuestInstance>();
        
        foreach (var questConfig in _config.quests)
        {
            quests.Add(CreateQuest(questConfig));
        }
        
        return quests;
    }
    
    private QuestInstance CreateQuest(QuestConfig config)
    {
        List<TaskInstance> tasks = new List<TaskInstance>();
        foreach (var taskConfig in config.tasks)
        {
            tasks.Add(CreateTask(taskConfig));
        }
        
        return new QuestInstance(config.questId, config.questDescription, tasks);
    }
    
    private TaskInstance CreateTask(TaskConfig config)
    {
        switch (config.TaskType)
        {
            case TaskType.Talk:
                return CreateTalkTask(config as TalkTaskConfig);
            case TaskType.Collect:
                return CreateGatherTask(config as CollectTaskConfig);
            case TaskType.SetInventory:
                return CreateSetInventoryTask(config as SetInventoryTaskConfig);
            default:
                throw new System.NotImplementedException();
        }
    }

    private TaskInstance CreateSetInventoryTask(SetInventoryTaskConfig config)
    {
        return new SetInventoryTaskInstance(config.InventoryItemTrigger);
    }

    private TalkTaskInstance CreateTalkTask(TalkTaskConfig config)
    {
        return new TalkTaskInstance(_dialogEvents, config.NpcId, config.DialogId, config.Description);
    }
    private CollectTaskInstance CreateGatherTask(CollectTaskConfig config)
    {
        return new CollectTaskInstance(_inventory, config.Item, config.RequiredCount);
    }
}
