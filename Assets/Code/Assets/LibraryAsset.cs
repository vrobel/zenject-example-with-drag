using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Assets
{
    [CreateAssetMenu(menuName = "Nomtec/Library asset")]
    public class LibraryAsset : ScriptableObject
    {
        [SerializeField] private Sprite thumbnail;
        [FormerlySerializedAs("listPrefab")] [SerializeField] private LibraryAssetItem listItemPrefab;
        [FormerlySerializedAs("prefab")] [SerializeField] private SceneItem sceneItemPrefab;

        public SceneItem SceneItemPrefab => sceneItemPrefab;

        public LibraryAssetItem ListItemPrefab => listItemPrefab;

        public class Factory
        {
        }
    }
}