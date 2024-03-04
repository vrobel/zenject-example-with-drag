using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using Zenject;

namespace Code
{
    public class BoxEaterHunting : MonoBehaviour
    {
        DisposableBag _disposable;
        
        [SerializeField] private SceneItem _sceneItem;
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider _trigger;

        [SerializeField] private float _speed = 1f;

        private SceneModel _sceneModel;
        private Vector3 _velocity;

        [Inject]
        private void Construct(SceneModel sceneModel)
        {
            _sceneModel = sceneModel;

            _sceneItem.IsActiveProperty.Subscribe(OnActiveStateChanged).AddTo(ref _disposable);
        }

        private void OnActiveStateChanged(bool isActive)
        {
            _animator.enabled = isActive;
            _trigger.enabled = isActive;
        }

        private void FixedUpdate()
        {
            if (!_sceneItem.isActive)
            {
                return;
            }
            
            var refPos = _sceneItem.transform.localPosition;
            SceneItem closestItem = null;
            var closestDistance = float.MaxValue;
            var first = true;
            foreach (var item in _sceneModel.SceneItemsReadonly)
            {
                if (item == _sceneItem)
                {
                    continue;
                }

                if (!item.isActive)
                {
                    continue;
                }

                if (first)
                {
                    first = false;
                    closestItem = item;
                    closestDistance = (closestItem.transform.localPosition - refPos)
                        .sqrMagnitude;
                    continue;
                }

                closestItem = (item.transform.localPosition - refPos).sqrMagnitude <
                              closestDistance
                    ? item
                    : closestItem;
                closestDistance = (closestItem.transform.localPosition - refPos).sqrMagnitude;
            }

            _animator.enabled = closestItem != null;
            if (closestItem != null)
            {
                _sceneItem.transform.LookAt(closestItem.transform.position, Vector3.up);
                _sceneItem.MoveBy((closestItem.transform.position -
                                   _sceneItem.transform.position).normalized * (_speed *
                    Time.fixedDeltaTime));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null) return;
            
            var sceneItem = other.attachedRigidbody.GetComponent<SceneItem>();
            if (sceneItem != null && sceneItem.isActive)
            {
                _sceneModel.DestroySceneItem(sceneItem);
            }
        }
    }
}