using System;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MergeElements.Views
{
    [RequireComponent(typeof(Canvas))]
    public class MergeWindowUI : MonoBehaviour
    {
        public event Action<Vector2> OnPointerDown;
        public event Action<Vector2> OnPointerDrag;
        public event Action<Vector2> OnPointerUp;
        public event Action OnClickSpawnButton;
        public event Action OnClickResetButton;
        public int MaxCount => _elementPositions.Length;
        public Camera WorldCamera => _canvas.worldCamera;

        [SerializeField]
        private RectTransform[] _elementPositions;
        [SerializeField]
        private Canvas _canvas;
        [Header("Buttons")]
        [SerializeField]
        private Button _spawnButton;
        [SerializeField]
        private Button _resetButton;

        private RectTransform _activeElement;
        private Vector2 _pointerOffset;
        

        private void OnValidate()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void Show()
        {
            _spawnButton.onClick.AddListener(() => OnClickSpawnButton?.Invoke());
            _resetButton.onClick.AddListener(() => OnClickResetButton?.Invoke());
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _spawnButton.onClick.RemoveAllListeners();
            _resetButton.onClick.RemoveAllListeners();
            OnClickSpawnButton = null;
            OnClickResetButton = null;
            OnPointerDown = null;
            OnPointerDrag = null;
            OnPointerUp = null;
            _canvas.enabled = false;
        }

        public void AddElement(RectTransform element, int index)
        {
            element.SetParent(_elementPositions[index]);
            element.offsetMin = Vector2.zero;
            element.offsetMax = Vector2.zero;
        }

        void Update()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.worldCamera,
                out localPoint
            );

            if (Input.GetMouseButtonDown(0))
                OnPointerDown?.Invoke(localPoint);

            if (Input.GetMouseButton(0))
                OnPointerDrag?.Invoke(localPoint);

            if (Input.GetMouseButtonUp(0))
                OnPointerUp?.Invoke(localPoint);
        }
    }
}
