using UnityEngine;
using UnityEngine.EventSystems;

namespace Code
{
    public class AssetTest : MonoBehaviour, IInitializePotentialDragHandler, IDragHandler
    {
        [SerializeField] private Transform draggedObject;
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            Debug.Assert(draggedObject, this);
            
            eventData.pointerDrag = eventData.selectedObject =
                draggedObject.gameObject;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //not used. only to trigger actual begin of Drag sequence.
        }
    }
}