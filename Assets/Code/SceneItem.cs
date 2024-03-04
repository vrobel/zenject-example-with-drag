using System;
using Code.Assets;
using Code.Signals;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code
{
    public class SceneItem : MonoBehaviour, IInitializePotentialDragHandler, IDragHandler
    {
        public readonly ReactiveProperty<bool> IsActiveProperty =
            new ReactiveProperty<bool>(true);
        
        [SerializeField] private Rigidbody rb;
        private SignalBus _signalBus;

        public bool isActive => IsActiveProperty.Value;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void MoveBy(Vector3 vector3)
        {
            rb.MovePosition(rb.position + vector3);
        }
        
        public void MoveTo(Vector3 position)
        {
            //todo: should be called from FixedUpdate
            rb.MovePosition(position);
        }

        #region IDrag

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            _signalBus.Fire(new DragSignal(this, eventData));
        }

        public void OnDrag(PointerEventData eventData)
        {
            //note: not in use. shouldn't be called
            Debug.LogError("Shouldn't be called");
        }

        #endregion

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