using System;
using R3;
using UnityEngine;
using Zenject;

namespace Code
{
    public class SceneItem : MonoBehaviour
    {
        public readonly ReactiveProperty<bool> IsActiveProperty =
            new ReactiveProperty<bool>(true);
        
        [SerializeField] private Rigidbody rb;
        
        public bool isActive => IsActiveProperty.Value;

        private void Awake()
        {
            IsActiveProperty.Subscribe(isActive => rb.isKinematic = !isActive).AddTo(this);
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