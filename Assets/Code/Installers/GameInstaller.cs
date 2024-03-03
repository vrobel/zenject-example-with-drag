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
    [SerializeField] private LibraryAsset testLibraryAsset;
    [SerializeField] private Transform sceneRoot;
    [SerializeField] private RectTransform libraryRoot;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.Bind<SceneModel>().AsSingle().WithArguments(sceneRoot);
        Container.Bind<LibraryModel>().AsSingle().WithArguments(libraryRoot);
        Container.Bind<PlacementSystem>().FromInstance(placementSystem).AsSingle();
        Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();

        Container.BindFactory<SceneItem, SceneItem, SceneItem.Factory>()
            .FromFactory<PrefabFactory<SceneItem>>();

        Container.BindFactory<LibraryAssetItem, LibraryAssetItem, LibraryAssetItem.Factory>()
            .FromFactory<PrefabFactory<LibraryAssetItem>>();

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