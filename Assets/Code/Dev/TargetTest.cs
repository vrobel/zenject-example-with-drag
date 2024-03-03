using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    ICancelHandler
{
    [SerializeField] private Transform draggedObject;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var eventDataSelectedObject =
            draggedObject == null ? gameObject : draggedObject.gameObject;
        eventData.selectedObject = eventDataSelectedObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RaycastHit hitInfo;
        var ray = Camera.main.ScreenPointToRay(eventData.position);
        var groundLayerMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, groundLayerMask))
        {
            draggedObject.GetComponent<Rigidbody>().MovePosition(hitInfo.point);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnCancel(BaseEventData eventData)
    {
        eventData.selectedObject = null;
    }
}