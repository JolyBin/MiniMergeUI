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
        private MergeElementUI _mergeElementUIPrefab;
        [SerializeField]
        private MergeElementSettings[] _orderElements;

        private void Start()
        {
            MergeController mergeController = new(_mergeWindowUI.MaxCount, _orderElements, _mergeElementUIPrefab, _mergeWindowUI);
            mergeController.ShowWindow();
        }
    }
}
