using System.Collections.Generic;
using Code.Assets;
using UnityEngine;
using Zenject;

namespace Code
{
    public class LibraryModel
    {
        private readonly List<LibraryAssetItem> _items = new List<LibraryAssetItem>();
        private Transform _root;
        private LibraryAssetItem.Factory _factory;

        [Inject]
        private void Construct(Transform root, LibraryAssetItem.Factory factory)
        {
            _factory = factory;
            _root = root;
        }

        public LibraryAssetItem CreateSceneItem(LibraryAsset libraryAsset)
        {
            Debug.Assert(libraryAsset);
            Debug.Assert(libraryAsset.SceneItemPrefab, libraryAsset);
            
            var item = _factory.Create(libraryAsset.ListItemPrefab);
            item.transform.SetParent(_root);
            _items.Add(item);
            return item;
        }
    
        public void DestroySceneItem(LibraryAssetItem item)
        {
            Debug.Assert(item);
            Debug.Assert(_items.Contains(item));
            
            _items.Remove(item);
            Object.Destroy(item.gameObject);
        }
    }
}