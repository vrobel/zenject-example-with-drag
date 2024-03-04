using Code;
using R3;

public interface IObservablePlacementSystem
{
    Observable<SceneItem> DraggedObject { get; }
}