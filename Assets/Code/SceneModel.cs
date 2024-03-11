using System.Collections.Generic;
using Code.Assets;
using ObservableCollections;
using UnityEngine;

namespace Code
{
    public class SceneModel
    {
        private readonly ObservableList<SceneItem> _sceneItems = new ObservableList<SceneItem>();
        private Transform _root;
        private SceneItem.Factory _factory;

        public IReadOnlyList<SceneItem> SceneItemsReadonly => _sceneItems;
        public IObservableCollection<SceneItem> SceneItemsObservableCollection => _sceneItems;

        private SceneModel(Transform root, SceneItem.Factory factory)
        {
            _factory = factory;
            _root = root;
            _sceneItems.AddRange(_root.GetComponentsInChildren<SceneItem>());
        }

        public SceneItem CreateSceneItem(LibraryAsset libraryAsset)
        {
            Debug.Assert(libraryAsset);
            Debug.Assert(libraryAsset.SceneItemPrefab, libraryAsset);
            
            var sceneItem = _factory.Create(libraryAsset.SceneItemPrefab);
            sceneItem.IsActiveProperty.Value = false;
            sceneItem.transform.SetParent(_root);
            _sceneItems.Add(sceneItem);
            return sceneItem;
        }
    
        public void DestroySceneItem(SceneItem sceneItem)
        {
            Debug.Assert(sceneItem);
            Debug.Assert(_sceneItems.Contains(sceneItem));
            
            _sceneItems.Remove(sceneItem);
            Object.Destroy(sceneItem.gameObject);
        }
    }
}
