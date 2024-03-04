using UnityEngine;

namespace Code.Assets
{
    [CreateAssetMenu(menuName = "Nomtec/Library asset")]
    public class LibraryAsset : ScriptableObject
    {
        [SerializeField] private string label;
        [SerializeField] private Sprite thumbnail;

        [SerializeField] private LibraryAssetItem listItemPrefab;

        [SerializeField] private SceneItem sceneItemPrefab;

        public SceneItem SceneItemPrefab => sceneItemPrefab;

        public LibraryAssetItem ListItemPrefab => listItemPrefab;

        public string Label => string.IsNullOrWhiteSpace(label) ? name : label;

        public Sprite Thumbnail => thumbnail;
    }
}