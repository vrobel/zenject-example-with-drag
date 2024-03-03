using Code;
using Code.Assets;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PlacementSystem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool IsDragging => _draggedObject != null;
    private SceneItem _draggedObject;
    private LibraryAsset _objectAsset;
    private SceneItem.Factory _sceneItemFactory;
    private Camera _mainCamera;

    [Inject]
    private void Inject(SceneItem.Factory sceneItemFactory, Camera mainCamera)
    {
        _sceneItemFactory = sceneItemFactory;
        _mainCamera = mainCamera;
    }

    public void InitializeDrag(SceneItem draggedObject, LibraryAsset objectAsset, PointerEventData eventData)
    {
        _draggedObject = draggedObject;
        _objectAsset = objectAsset;

        if (draggedObject != null)
        {
            eventData.selectedObject = draggedObject.gameObject;
        }

        eventData.pointerDrag = gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if dragged object is missing, needs to be spawned.
        if (_draggedObject == null)
        {
            _draggedObject = _sceneItemFactory.Create(_objectAsset.SceneItemPrefab);
        }

        var eventDataSelectedObject = _draggedObject.gameObject;
        eventData.selectedObject = eventDataSelectedObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var ray = _mainCamera.ScreenPointToRay(eventData.position);
        var groundLayerMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, groundLayerMask))
        {
            _draggedObject.MoveTo(hitInfo.point);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _draggedObject = null;
    }

    public void OnCancel(BaseEventData eventData)
    {
        eventData.selectedObject = null;
        Destroy(_draggedObject.gameObject);
        _draggedObject = null;
    }
}