using UnityEngine;
using Zenject;

public class BindingsInstaller : MonoInstaller
{
    [SerializeField] private NpcRegistry _npcRegistry;
    [SerializeField] private QuestObjectsSceneRegistry _questObjectsSceneRegistry;
    
    public override void InstallBindings()
    {
        Container.Bind<NpcRegistry>().FromInstance(_npcRegistry).AsSingle().NonLazy();
        Container.Bind<QuestObjectsSceneRegistry>().FromInstance(_questObjectsSceneRegistry).AsSingle().NonLazy();
        
        Container.Bind<ISceneChanger>().To<SceneChangerService>().AsSingle();
        Container.Bind<IGameState>().To<GameStateService>().AsSingle();
        Container.Bind<IInputControler>().To<InputControlerService>().AsSingle();
        Container.Bind<IDialogueSetter>().To<DialogueService>().AsSingle();
        Container.Bind<ITaskContainer>().To<TaskService>().AsSingle();
        GameSaver.StopAllSaves = false;
    }
}
