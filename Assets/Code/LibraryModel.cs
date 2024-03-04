using System;
using System.Collections.Generic;
using Code.Assets;
using UnityEngine;
using Zenject;

namespace Code
{
    public class LibraryModel : IInitializable
    {
        private readonly List<LibraryAssetItem> _items = new List<LibraryAssetItem>();
        private RectTransform _root;
        private LibraryAssetItem.Factory _factory;
        private Library _library;

        [Inject]
        private void Construct(RectTransform root, Library library, LibraryAssetItem.Factory factory)
        {
            _root = root;
            _library = library;
            _factory = factory;
            _items.AddRange(_root.GetComponentsInChildren<LibraryAssetItem>());
        }

        public void Initialize()
        {
            CreateLibrary(_library);
            ResetFilter();
        }

        private void CreateLibrary(Library library)
        {
            foreach (var libraryAsset in library.AssetsReadonly)
            {
                //todo: factory creates on scene, create directly in provided _root 
                var item = _factory.Create(libraryAsset.ListItemPrefab, libraryAsset);
                item.transform.SetParent(_root);
                item.transform.localScale = Vector3.one;
                _items.Add(item);
            }
        }

        public void SetFilter(string filter)
        {
            var hasFilter = !string.IsNullOrWhiteSpace(filter);
            foreach (var libraryAssetItem in _items)
            {
                var label = libraryAssetItem.Label;
                var hasLabel = !string.IsNullOrWhiteSpace(label);
                var setActive = true;
                if (hasFilter && hasLabel)
                {
                    setActive = label.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
                }
                libraryAssetItem.SetActive(setActive);
            }
        }

        public void ResetFilter()
        {
            SetFilter(null);
        }
    }
}