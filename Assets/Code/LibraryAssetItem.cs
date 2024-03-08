using Code.Assets;
using Code.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code
{
    public class LibraryAssetItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private LibraryAsset libraryAssetReference;
        [SerializeField] private Image image;
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
                image.sprite = libraryAssetReference.Thumbnail;
            }
        }

        public void SetActive(bool setActive)
        {
            gameObject.SetActive(setActive);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _signalBus.Fire(new DragSignal(libraryAssetReference, eventData));
        }

        public sealed class Factory : PlaceholderFactory<LibraryAssetItem, LibraryAsset, LibraryAssetItem>
        {
        }
    }
}