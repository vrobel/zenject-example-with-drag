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
    private SceneModel _sceneModel;
    private Camera _mainCamera;

    [Inject]
    private void Inject(SceneModel sceneModel, Camera mainCamera)
    {
        _sceneModel = sceneModel;
        _mainCamera = mainCamera;
    }

    public void InitializeDrag(SceneItem draggedObject, LibraryAsset objectAsset,
        PointerEventData eventData)
    {
        _draggedObject = draggedObject;
        _objectAsset = objectAsset;

        if (draggedObject != null)
        {
            eventData.selectedObject = draggedObject.gameObject;
        }

        eventData.pointerDrag = gameObject;
    }

    #region IDrag

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if dragged object is missing, needs to be spawned.
        if (_draggedObject == null)
        {
            _draggedObject = _sceneModel.CreateSceneItem(_objectAsset);
        }

        eventData.selectedObject = _draggedObject.gameObject;
        MoveToPosition(eventData.position, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var eventDataPosition = eventData.position;
        MoveToPosition(eventDataPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _draggedObject = null;
    }

    public void OnCancel(BaseEventData eventData)
    {
        eventData.selectedObject = null;
        if (_draggedObject != null) _sceneModel.DestroySceneItem(_draggedObject);
        _draggedObject = null;
    }

    #endregion

    private void MoveToPosition(Vector2 eventDataPosition,
        bool includeTransformMovement = false)
    {
        var ray = _mainCamera.ScreenPointToRay(eventDataPosition);
        var groundLayerMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, groundLayerMask))
        {
            if (includeTransformMovement)
            {
                _draggedObject.transform.position = hitInfo.point;
            }

            _draggedObject.MoveTo(hitInfo.point);
        }
    }
}