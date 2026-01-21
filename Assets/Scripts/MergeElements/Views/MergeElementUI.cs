using UnityEngine;
using UnityEngine.UI;

namespace Core.MergeElements.Views
{
    [RequireComponent(typeof(RectTransform))]
    public class MergeElementUI : MonoBehaviour
    {
        [field: SerializeField]
        public RectTransform RectTransform {  get; private set; }

        [SerializeField]
        private Image _icon;

        private Canvas _mainWindow;

        private void OnValidate()
        {
            RectTransform = GetComponent<RectTransform>();
        }
        public void SetVisualParams(Sprite sprite) => _icon.sprite = sprite;
        public void Init() => _mainWindow = GetComponentInParent<Canvas>();
    }
}
