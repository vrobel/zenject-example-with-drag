using System.Collections.Generic;
using UnityEngine;

namespace Code.Assets
{
    [CreateAssetMenu(menuName = "Nomtec/Library")]
    public class Library : ScriptableObject
    {
        [SerializeField] private List<LibraryAsset> assets;
    }
}