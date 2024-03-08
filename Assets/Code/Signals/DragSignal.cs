using Code.Assets;
using UnityEngine.EventSystems;

namespace Code.Signals
{
    public class DragSignal
    {
        public DragSignal(LibraryAsset objectAsset)
        {
            ObjectAsset = objectAsset;
        }

        public DragSignal(SceneItem draggedObject)
        {
            DraggedObject = draggedObject;
        }

        public SceneItem DraggedObject { get; }
        public LibraryAsset ObjectAsset { get; }
    }
}