using System;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MergeElements.Views
{
    [RequireComponent(typeof(Canvas))]
    public class WinWindowUI : MonoBehaviour
    {
        public event Action OnClickRestartButton;

        [SerializeField]
        private Canvas _canvas;
        [Header("Buttons")]
        [SerializeField]
        private Button _restartButton;      

        private void OnValidate()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void Show()
        {
            _restartButton.onClick.AddListener(() => OnClickRestartButton?.Invoke());
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _restartButton.onClick.RemoveAllListeners();
            OnClickRestartButton = null;
            _canvas.enabled = false;
        }
    }
}
