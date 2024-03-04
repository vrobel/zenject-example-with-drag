using System.Collections.Generic;
using Code;
using Code.Assets;
using Code.Signals;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Library library;
    [SerializeField] private Transform sceneRoot;
    [SerializeField] private RectTransform libraryRoot;
    [SerializeField] private LibraryModelFilter libraryModelFilter;
    [SerializeField] private LibraryPanelSlideAnimator libraryPanelSlideAnimator;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.Bind<Library>().FromInstance(library).AsSingle();
        Container.Bind<SceneModel>().AsSingle().WithArguments(sceneRoot).NonLazy();
        Container.BindInterfacesAndSelfTo<LibraryModel>().AsSingle().WithArguments(libraryRoot)
            .NonLazy();
        Container.Bind<LibraryModelFilter>().FromInstance(libraryModelFilter).AsSingle();
        Container.BindInterfacesAndSelfTo<LibraryPanelSlideAnimator>().FromInstance(libraryPanelSlideAnimator).AsSingle();
        Container.Bind<PlacementSystem>().FromInstance(placementSystem).AsSingle();
        Container.Bind<IObservablePlacementSystem>().To<PlacementSystem>().FromResolve();
        Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();

        Container.BindFactory<SceneItem, SceneItem, SceneItem.Factory>()
            .FromFactory<PrefabFactory<SceneItem>>();

        Container
            .BindFactory<LibraryAssetItem, LibraryAsset, LibraryAssetItem,
                LibraryAssetItem.Factory>()
            .FromFactory<
                PrefabFactory<LibraryAsset, LibraryAssetItem>>();

        Container.DeclareSignal<DragSignal>();
        Container.BindSignal<DragSignal>()
            .ToMethod<PlacementSystem>((system, signal) =>
                system.InitializeDrag(signal.DraggedObject, signal.ObjectAsset,
                    signal.PointerEvent))
            .FromResolve();
    }

    public override void Start()
    {
        base.Start();
        
    }
}