using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MergeElements.Views
{
    [RequireComponent(typeof(Canvas))]
    public class MergeWindowUI : MonoBehaviour
    {
        public event Action OnClickSpawnButton;
        public event Action OnClickResetButton;
        public int MaxCount => _elementPositions.Length;

        [SerializeField]
        private RectTransform[] _elementPositions;
        [SerializeField]
        private Canvas _canvas;
        [Header("Buttons")]
        [SerializeField]
        private Button _spawnButton;
        [SerializeField]
        private Button _resetButton;

        private void OnValidate()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void Show()
        {
            _spawnButton.onClick.AddListener(() => OnClickSpawnButton.Invoke());
            _resetButton.onClick.AddListener(() => OnClickResetButton.Invoke());
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _spawnButton.onClick.RemoveAllListeners();
            _resetButton.onClick.RemoveAllListeners();
            OnClickSpawnButton = null;
            OnClickResetButton = null;
            _canvas.enabled = false;
        }

        public void ShowElement(RectTransform element, int index)
        {
            element.SetParent(_elementPositions[index]);
            element.offsetMin = Vector2.zero;
            element.offsetMax = Vector2.zero;
        }
    }
}
