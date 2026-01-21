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
        private MergeElementUI _mergewElementUIPrefab;
        private MergeElementUI _tempElementUI;

        private Vector2 _activeElementPointerOffset;
        private MergeCell _selectbleCalle;


        public MergeController(int maxElementsCount, MergeElementSettings[] orderElemets, MergeElementUI mergewElementUIPrefab, MergeWindowUI mergeWindowUI)
        {
            _orderElements = orderElemets;
            _elementsGrid = new MergeCell[maxElementsCount];

            _mergeWindowUI = mergeWindowUI;
            _mergewElementUIPrefab = mergewElementUIPrefab;

            Init();
        }

        public void Init()
        {
            for (int i = 0; i < _elementsGrid.Length; i++)
            {
                MergeCell cell = new(GameObject.Instantiate(_mergewElementUIPrefab));

                _mergeWindowUI.AddElement(cell.View.RectTransform, i);
                _elementsGrid[i] = cell;
            }

            MergeElementUI lastElem = _elementsGrid[_elementsGrid.Length - 1].View;
            _tempElementUI = GameObject.Instantiate(_mergewElementUIPrefab);
            _tempElementUI.RectTransform.SetParent(lastElem.RectTransform.parent);
            _tempElementUI.RectTransform.offsetMin = Vector2.zero;
            _tempElementUI.RectTransform.offsetMax = Vector2.zero;
            _tempElementUI.gameObject.SetActive(false);
        }

        public void ShowWindow()
        {
            _mergeWindowUI.Show();
            Reset();
            _mergeWindowUI.OnClickSpawnButton += TryAddElement;
            _mergeWindowUI.OnClickResetButton += Reset;
            _mergeWindowUI.OnPointerDown += HandlePointerDown;
            _mergeWindowUI.OnPointerDrag += HandlePointerDrag;
            _mergeWindowUI.OnPointerUp += HandlePointerUp;
        }

        public void HideWindo()
        {
            _mergeWindowUI.Hide();
            Reset();
        }

        private void Reset()
        {
            ResetSelectbleElement();
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

        private bool TryGetCellOnPosition(Vector2 pointerPos, out MergeCell mergeCell)
        {
            mergeCell = null;
            foreach (var element in _elementsGrid)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(element.View.RectTransform, Input.mousePosition, _mergeWindowUI.WorldCamera))
                {
                    mergeCell = element;
                    return true;
                }
            }
            return false;
        }

        private void HandlePointerDown(Vector2 pointerPos)
        {
            ResetSelectbleElement();

            if (TryGetCellOnPosition(pointerPos, out MergeCell cell) && cell.Model != null)
            {
                _selectbleCalle = cell;
                _tempElementUI.transform.position = _selectbleCalle.View.transform.position;
                _tempElementUI.SetVisualParams(_selectbleCalle.Model.Icon);
                _selectbleCalle.View.gameObject.SetActive(false);
                _tempElementUI.gameObject.SetActive(true);


                _activeElementPointerOffset = _tempElementUI.RectTransform.anchoredPosition - pointerPos;
            }
        }

        private void HandlePointerDrag(Vector2 pointerPos)
        {
            if (_selectbleCalle == null) return;
            _tempElementUI.RectTransform.anchoredPosition = pointerPos + _activeElementPointerOffset;
        }

        private void HandlePointerUp(Vector2 pointerPos)
        {
            if (_selectbleCalle == null) return;

            if(TryGetCellOnPosition(pointerPos, out MergeCell cell))
            {
                if(cell.Model == null)
                {
                    cell.Model = _selectbleCalle.Model;
                    _selectbleCalle.Model = null;
                    _selectbleCalle = null;
                    _tempElementUI.gameObject.SetActive(false);
                    return;
                }
            }
            ResetSelectbleElement();
        }

        private void ResetSelectbleElement()
        {
            _tempElementUI.gameObject.SetActive(false);
            if (_selectbleCalle != null)
            {
                _selectbleCalle.View.gameObject.SetActive(true);
                _selectbleCalle = null;
            }
            
        }
    }
}
