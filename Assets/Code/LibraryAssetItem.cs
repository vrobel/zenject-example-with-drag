using Code.Assets;
using Code.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code
{
    public class LibraryAssetItem : MonoBehaviour, IInitializePotentialDragHandler, IDragHandler
    {
        [SerializeField] private LibraryAsset libraryAssetReference;
        private SignalBus _signalBus;

        public LibraryAsset LibraryAssetReference => libraryAssetReference;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            _signalBus.Fire(new DragSignal(libraryAssetReference, eventData));
        }

        public void OnDrag(PointerEventData eventData)
        {
            //note: not in use. shouldn't be called
            Debug.LogError("Shouldn't be called");
        }

        public sealed class Factory : PlaceholderFactory<LibraryAssetItem, LibraryAssetItem>
        {
        }
    }
}