using System;
using Code.Assets;
using Code.Signals;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code
{
    public class SceneItem : MonoBehaviour
    {
        public readonly ReactiveProperty<bool> IsActiveProperty =
            new ReactiveProperty<bool>(true);
        
        [SerializeField] private Rigidbody rb;
        
        private DisposableBag _disposableBag;

        public bool isActive => IsActiveProperty.Value;

        private void Awake()
        {
            IsActiveProperty.Subscribe(isActive => rb.isKinematic = !isActive).AddTo(ref _disposableBag);
        }

        private void OnDestroy()
        {
            _disposableBag.Dispose();
        }

        [Inject]
        private void Construct()
        {
        }

        public void MoveBy(Vector3 vector3)
        {
            MoveTo(rb.position + vector3);
        }
        
        public void MoveTo(Vector3 position)
        {
            rb.position = position;
        }

        private void OnValidate()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
        }

        public sealed class Factory : PlaceholderFactory<SceneItem, SceneItem>
        {
        }
    }
}