using Code.Assets;
using UnityEngine.EventSystems;

namespace Code.Signals
{
    public class DragSignal
    {
        public DragSignal(LibraryAsset objectAsset, PointerEventData pointerEvent)
        {
            ObjectAsset = objectAsset;
            PointerEvent = pointerEvent;
        }

        public DragSignal(SceneItem draggedObject, PointerEventData pointerEvent)
        {
            DraggedObject = draggedObject;
            PointerEvent = pointerEvent;
        }

        public SceneItem DraggedObject { get; }
        public LibraryAsset ObjectAsset { get; }
        public PointerEventData PointerEvent { get; }
    }
}