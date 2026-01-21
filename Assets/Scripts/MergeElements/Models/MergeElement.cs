using UnityEngine;

namespace Core.MergeElements.Models
{
    public class MergeElement
    {
        public int ID { get; private set; }
        public Sprite Icon { get; private set; }

        public MergeElement NextMergeElement { get; private set; }

        public MergeElement(int id, Sprite icon, MergeElement nextmergElement)
        {
            ID = id;
            Icon = icon;
            NextMergeElement = nextmergElement;
        }
    }
}
