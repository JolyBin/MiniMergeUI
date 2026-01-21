using UnityEngine;

namespace Core.MergeElements.Models
{
    [CreateAssetMenu(fileName = "Merge Element", menuName = "Merge Elements/Create Merge Element")]
    public class MergeElementSettings : ScriptableObject
    {
        [field:SerializeField]
        public int ID {  get; private set; }
        [field:SerializeField]
        public Sprite Icon { get; private set; }

        public MergeElement GetMergeElement() => new MergeElement(ID, Icon);

    }
}
