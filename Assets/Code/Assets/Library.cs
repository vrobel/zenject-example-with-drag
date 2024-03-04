using System.Collections.Generic;
using UnityEngine;

namespace Code.Assets
{
    [CreateAssetMenu(menuName = "Nomtek/Library")]
    public class Library : ScriptableObject
    {
        [SerializeField] private List<LibraryAsset> assets;

        public IReadOnlyList<LibraryAsset> AssetsReadonly => assets.AsReadOnly();
    }
}