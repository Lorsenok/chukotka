using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private QuestLineConfig questLine;
    
    public override void InstallBindings()
    {
        Container.Bind<QuestLineConfig>().FromInstance(questLine).AsSingle().NonLazy();
        Container.BindInterfacesTo<QuestManager>().AsSingle().NonLazy();
    }
}
