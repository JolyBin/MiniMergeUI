using Core.MergeElements.Controllers;
using Core.MergeElements.Models;
using Core.MergeElements.Views;
using UnityEngine;

namespace Core
{
    public class TempGameManager : MonoBehaviour
    {
        [SerializeField]
        private MergeWindowUI _mergeWindowUI;
        [SerializeField]
        private WinWindowUI _winWindowUI;
        [SerializeField]
        private MergeElementUI _mergeElementUIPrefab;
        [SerializeField]
        private MergeElementSettings _firstElement;

        private MergeController _mergeController;
        private WinController _winController;

        private void Start() => Init();

        private void Init()
        {
            _mergeController = new(_mergeWindowUI.MaxCount, _firstElement, _mergeElementUIPrefab, _mergeWindowUI);
            _winController = new WinController(_winWindowUI);

            Restart();
        }

        private void StartGame()
        {
            _mergeController.ShowWindow();
            _mergeController.OnGameOver += () => _winController.ShowWindow();
            _winController.OnCloseWindow += Restart;
        }

        private void Restart()
        {
            _winController.HideWindow();
            _mergeController.HideWindow();
            StartGame();
        }
    }
}
