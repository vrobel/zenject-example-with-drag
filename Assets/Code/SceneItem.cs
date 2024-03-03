using System;
using Code.Assets;
using Code.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code
{
    public class SceneItem : MonoBehaviour, IInitializePotentialDragHandler, IDragHandler
    {
        [SerializeField] private Rigidbody rb;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void MoveTo(Vector3 position)
        {
            //todo: should be called from FixedUpdate
            rb.MovePosition(position);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            _signalBus.Fire(new DragSignal(this, eventData));
        }

        public void OnDrag(PointerEventData eventData)
        {
            //note: not in use. shouldn't be called
            Debug.LogError("Shouldn't be called");
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