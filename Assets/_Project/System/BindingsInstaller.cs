using UnityEngine;
using Zenject;

public class BindingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISceneChanger>().To<SceneChangerService>().AsSingle();
        Container.Bind<IGameState>().To<GameStateService>().AsSingle();
        Container.Bind<IInputControler>().To<InputControlerService>().AsSingle();
        Container.Bind<IDialogueSetter>().To<DialogueService>().AsSingle();
        Container.Bind<ISaver>().To<SaverService>().AsSingle();
        Container.Bind<IInventory>().To<InventoryService>().AsSingle();
        Container.Bind<ITaskContainer>().To<TaskService>().AsSingle();
        Container.Bind<IAbilityContainer>().To<AbilityService>().AsSingle();
    }
}
