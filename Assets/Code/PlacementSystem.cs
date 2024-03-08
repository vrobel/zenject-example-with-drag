using System;
using Code;
using Code.Assets;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class PlacementSystem : MonoBehaviour, IObservablePlacementSystem, IPointerClickHandler, ICancelHandler
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

    public void InitializeDrag(SceneItem draggedObject, LibraryAsset objectAsset, PointerEventData eventData)
    {
        if (_draggedObjectProperty.Value != null)
        {
            CancelDrag();
            return;
        }
        
        _draggedObjectProperty.Value = draggedObject;
        _objectAsset = objectAsset;

        if (draggedObject != null)
        {
            draggedObject.IsActiveProperty.Value = false;
        }

        //if dragged object is missing, needs to be spawned.
        if (_draggedObjectProperty.Value == null)
        {
            _draggedObjectProperty.Value = _sceneModel.CreateSceneItem(_objectAsset);
        }

        eventData.selectedObject = gameObject;
        
        //initial position
        var plane = new Plane(transform.up, transform.position);
        var ray = ScreenPointToRay();
        plane.Raycast(ray, out var enter);
        _draggedObjectProperty.Value.transform.position = ray.GetPoint(enter);

        UpdateDraggedObject(true);
    }

    private void TryCompleteDrag()
    {
        if (_draggedObjectProperty.Value != null)
        {
            UpdateDraggedObject();
            _draggedObjectProperty.Value.IsActiveProperty.Value = true;
            _draggedObjectProperty.Value.MoveBy(Vector3.up * .2f);
            _draggedObjectProperty.Value = null;
        }
    }

    private void CancelDrag()
    {
        if (_draggedObjectProperty.Value != null)
            _sceneModel.DestroySceneItem(_draggedObjectProperty.Value);
        _draggedObjectProperty.Value = null;
    }

    public void FixedUpdate()
    {
        UpdateDraggedObject();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.selectedObject = null;
        TryCompleteDrag();
    }

    public void OnCancel(BaseEventData eventData)
    {
        eventData.selectedObject = null;
        CancelDrag();
    }

    private void UpdateDraggedObject(bool includeTransformMovement = false)
    {
        if (_draggedObjectProperty.Value == null)
        {
            return;
        }

        var ray = ScreenPointToRay();
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

    private Ray ScreenPointToRay()
    {
        return _mainCamera.ScreenPointToRay(Mouse.current.position.value);
    }
}