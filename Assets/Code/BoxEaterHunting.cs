using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

namespace Code
{
    public class BoxEaterHunting : MonoBehaviour
    {
        [SerializeField] private SceneItem _sceneItem;
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider _trigger;

        [SerializeField] private float _speed = 1f;

        private SceneModel _sceneModel;
        [SerializeField, Range(0f,1f)] private float maxRadiansDelta;
        private ISynchronizedView<SceneItem, SceneItemPreyView> _preySceneItems;

        private class SceneItemPreyView
        {
            public readonly bool IsPrey;

            public SceneItemPreyView(SceneItem item)
            {
                IsPrey = item.TryGetComponent<Prey>(out _);
            }
        }

        [Inject]
        private void Construct(SceneModel sceneModel)
        {
            _sceneModel = sceneModel;
        }

        private void Start()
        {
            _sceneItem.IsActiveProperty.Subscribe(OnActiveStateChanged).AddTo(this);
            _preySceneItems = _sceneModel.SceneItemsObservableCollection.CreateView(
                item =>
                {
                    var view = new SceneItemPreyView(item);
                    return !view.IsPrey || item == _sceneItem ? null : view;
                });
            _preySceneItems.AttachFilter(PreyDynamicFilter);
        }

        private void FixedUpdate()
        {
            if (!_sceneItem.isActive)
            {
                return;
            }

            var hasAny = _preySceneItems.Any();
            var closestItem = hasAny ? _preySceneItems.Aggregate(FindCloserPrey).Value : null;
            
            _animator.enabled = hasAny;
            if (hasAny)
            {
                FollowPrey(closestItem);
            }
        }

        private (SceneItem, SceneItemPreyView) FindCloserPrey((SceneItem, SceneItemPreyView) i1, (SceneItem, SceneItemPreyView) i2)
        {
            var hunterPosition = _sceneItem.transform.localPosition;
            return (i1.Item1.transform.localPosition - hunterPosition).sqrMagnitude <
                   (i2.Item1.transform.localPosition - hunterPosition).sqrMagnitude
                ? i1
                : i2;
        }

        private void OnActiveStateChanged(bool isActive)
        {
            _animator.enabled = isActive;
            _trigger.enabled = isActive;
        }

        private bool PreyDynamicFilter(SceneItem item, SceneItemPreyView view)
        {
            if (view == null)
            {
                return false;
            }
            
            if (!item.isActive)
            {
                return false;
            }

            var sameLevel = Mathf.Abs(item.transform.localPosition.y - transform.localPosition.y) < .1f;
            if (!sameLevel)
            {
                return false;
            }

            return true;
        }

        private void FollowPrey(SceneItem closestItem)
        {
            var rotateTowards =
                Vector3.RotateTowards(_sceneItem.transform.forward,
                    closestItem.transform.position - _sceneItem.transform.position,
                    maxRadiansDelta * Mathf.PI, float.MaxValue);
            _sceneItem.transform.rotation =
                Quaternion.LookRotation(rotateTowards, Vector3.up);

            _sceneItem.MoveBy((closestItem.transform.position -
                               _sceneItem.transform.position).normalized * (_speed *
                Time.fixedDeltaTime));
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