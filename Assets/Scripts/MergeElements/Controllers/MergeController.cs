using System.Linq;
using System.Xml.Linq;
using Core.MergeElements.Models;
using Core.MergeElements.Views;
using UnityEngine;

namespace Core.MergeElements.Controllers
{
    public class MergeController
    {
        private MergeElementSettings[] _orderElements;
        private MergeCell[] _elementsGrid;

        //UI
        private MergeWindowUI _mergeWindowUI;
        private MergeElementUI _mergewUIElementPrefab;
        
        public MergeController(int maxElementsCount, MergeElementSettings[] orderElemets, MergeElementUI mergewUIElementPrefab, MergeWindowUI mergeWindowUI)
        {
            _orderElements = orderElemets;
            _elementsGrid = new MergeCell[maxElementsCount];

            _mergeWindowUI = mergeWindowUI;
            _mergewUIElementPrefab = mergewUIElementPrefab;

            Init();
        }

        public void Init()
        {
            foreach (MergeCell cell in _elementsGrid) 
            {

            }
            for (int i = 0; i < _elementsGrid.Length; i++)
            {
                MergeCell cell = new();
                cell.View = GameObject.Instantiate(_mergewUIElementPrefab);
                cell.Model = null;

                _mergeWindowUI.ShowElement(cell.View.RectTransform, i);
                cell.View.gameObject.SetActive(false);

                _elementsGrid[i] = cell;
            }
        }

        public void ShowWindow()
        {
            _mergeWindowUI.Show();
            _mergeWindowUI.OnClickSpawnButton += TryAddElement;
            _mergeWindowUI.OnClickResetButton += Reset;
        }

        public void HideWindo()
        {
            _mergeWindowUI.Hide();
        }

        private void Reset()
        {
            foreach (MergeCell cell in _elementsGrid)
            {
                cell.Model = null;
                cell.View.gameObject.SetActive(false);
            }
        }

        private void TryAddElement()
        {            
            var emptyElements = _elementsGrid
                .Select((value, index) => new { value, index })
                .Where(x => x.value.Model == null)
                .Select(x => x.index).ToArray();
            
            if (emptyElements.Length == 0)
                return;

            int randomindex = emptyElements[Random.Range(0, emptyElements.Length)];
            MergeElement newElement = _orderElements[0].GetMergeElement();

            var cell = _elementsGrid[randomindex];
            cell.Model = newElement;
            cell.View.SetVisualParams(newElement.Icon);
            cell.View.gameObject.SetActive(true);
        }
    }
}
