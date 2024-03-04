using Code.Assets;
using Code.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code
{
    public class LibraryAssetItem : MonoBehaviour, IInitializePotentialDragHandler, IDragHandler
    {
        [SerializeField] private LibraryAsset libraryAssetReference;
        [SerializeField] private TextMeshProUGUI label;
        private SignalBus _signalBus;

        public LibraryAsset LibraryAssetReference => libraryAssetReference;

        public string Label => libraryAssetReference == null
            ? gameObject.name
            : libraryAssetReference.Label;

        public Sprite Thumbnail => libraryAssetReference == null
            ? null
            : libraryAssetReference.Thumbnail;

        [Inject]
        private void Construct(SignalBus signalBus, [InjectOptional] LibraryAsset libraryAsset)
        {
            libraryAssetReference = libraryAsset != null ? libraryAsset : libraryAssetReference;
            
            _signalBus = signalBus;
            
            if (libraryAssetReference != null)
            {
                label.text = libraryAssetReference.Label;
            }
        }

        public void SetActive(bool setActive)
        {
            gameObject.SetActive(setActive);
        }

        #region IDrag

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            _signalBus.Fire(new DragSignal(libraryAssetReference, eventData));
        }

        public void OnDrag(PointerEventData eventData)
        {
            //note: not in use. shouldn't be called
            Debug.LogError("Shouldn't be called");
        }

        #endregion

        public sealed class Factory : PlaceholderFactory<LibraryAssetItem, LibraryAsset, LibraryAssetItem>
        {
        }
    }
}