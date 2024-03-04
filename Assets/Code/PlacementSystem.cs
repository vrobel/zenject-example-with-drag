using System;
using Code;
using Code.Assets;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PlacementSystem : MonoBehaviour, IObservablePlacementSystem, IBeginDragHandler,
    IDragHandler, IEndDragHandler
{
    private readonly ReactiveProperty<SceneItem> _draggedObjectProperty =
        new ReactiveProperty<SceneItem>();

    private LibraryAsset _objectAsset;
    private SceneModel _sceneModel;
    private Camera _mainCamera;

    public Observable<SceneItem> DraggedObjectObservable => _draggedObjectProperty;

    [Inject]
    private void Inject(SceneModel sceneModel, Camera mainCamera)
    {
        _sceneModel = sceneModel;
        _mainCamera = mainCamera;
    }

    public void InitializeDrag(SceneItem draggedObject, LibraryAsset objectAsset,
        PointerEventData eventData)
    {
        _draggedObjectProperty.Value = draggedObject;
        _objectAsset = objectAsset;

        if (draggedObject != null)
        {
            draggedObject.IsActiveProperty.Value = false;
            eventData.selectedObject = draggedObject.gameObject;
        }

        eventData.pointerDrag = gameObject;
    }

    #region IDrag

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if dragged object is missing, needs to be spawned.
        if (_draggedObjectProperty.Value == null)
        {
            _draggedObjectProperty.Value = _sceneModel.CreateSceneItem(_objectAsset);
        }

        eventData.selectedObject = _draggedObjectProperty.Value.gameObject;
        MoveToPosition(eventData.position, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var eventDataPosition = eventData.position;
        MoveToPosition(eventDataPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _draggedObjectProperty.Value.IsActiveProperty.Value = true;
        _draggedObjectProperty.Value.MoveBy(Vector3.up * .2f);
        _draggedObjectProperty.Value = null;
    }

    public void OnCancel(BaseEventData eventData)
    {
        eventData.selectedObject = null;
        if (_draggedObjectProperty.Value != null)
            _sceneModel.DestroySceneItem(_draggedObjectProperty.Value);
        _draggedObjectProperty.Value = null;
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
                _draggedObjectProperty.Value.transform.position = hitInfo.point;
            }

            _draggedObjectProperty.Value.MoveTo(hitInfo.point);
        }
    }
}