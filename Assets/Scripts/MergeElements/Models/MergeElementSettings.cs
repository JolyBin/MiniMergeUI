using UnityEngine;

namespace Core.MergeElements.Models
{
    [CreateAssetMenu(fileName = "Merge Element", menuName = "Merge Elements/Create Merge Element")]
    public class MergeElementSettings : ScriptableObject
    {
        [SerializeField]
        private int _id;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private MergeElementSettings _nextElement;

        public MergeElement GetMergeElement() => new MergeElement(_id, _icon, _nextElement?.GetMergeElement());

    }
}
