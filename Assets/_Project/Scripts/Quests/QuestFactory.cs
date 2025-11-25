using System.Collections.Generic;
using Zenject;

public class QuestFactory
{
    private readonly QuestLineConfig _config;

    private readonly QuestDialogEvents _dialogEvents;
    
    public QuestFactory(QuestLineConfig config, QuestDialogEvents questDialogEvents)
    {
        _config = config;
        _dialogEvents = questDialogEvents;
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
            default:
                throw new System.NotImplementedException();
        }
    }
    
    private TalkTaskInstance CreateTalkTask(TalkTaskConfig config)
    {
        return new TalkTaskInstance(_dialogEvents, config.NpcId, config.DialogId, config.Description);
    }
}
